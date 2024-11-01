using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateMachine : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float groundDrag;
    float moveSpeed;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode interactKey = KeyCode.E;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    public LayerMask groundLayer;
    bool grounded;

    [Header("Puzzle")]
    public List<string> KeysInventory;

    [Header("Configs")]
    [SerializeField] Transform orientation;

    // inputs
    float horizontalInput;
    float verticalInput;

    // configs
    public float playerHealth;
    public float playerPower;
    public bool canUseUltimate;

    Vector3 flatVel;

    // state variables
    public static PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // respawn
    (Vector3, quaternion) InitialPosition;

    // all sprite references
    List<SpritesLookAt> spritesLookAtList = new();

    // public declarations
    public Vector3 moveDirection;
    static NavMeshAgent agent;
    static Rigidbody rb;
    static Collider _collider;
    static SpriteRenderer _spriteRenderer;

    public static bool isRespawning;
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }

    private void Awake()
    {
        _states = new(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        PlayerPrefs.SetString("isPlayerReadyToAttack_Ranged", "true");
        PlayerPrefs.SetString("isPlayerReadyToAttack", "true");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ResumePlayer();

        // set defaults
        InitialPosition = (transform.position, transform.rotation);
        playerHealth = PlayerConfigs.Instance.playerHealth;
        playerPower = PlayerConfigs.Instance.playerPower;
        KeysInventory = new();

        Time.timeScale = 1;

        UpdateHealthBar();
        UpdatePowerBar();

        spritesLookAtList = FindObjectsOfType<SpritesLookAt>().ToList();
    }

    void Update()
    {
        if (PlayerConfigs.Instance.IsGameOver) return;
        if (!PlayerConfigs.Instance.CanMove) return;
        if (isRespawning) return;

        // states
        _currentState.UpdateStates();

        // TODO: PLAYER HURT SPRITE COLOR

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.6f, groundLayer);

        if (hit.collider != null)
        {
            Vector3 movePos = transform.position;
            movePos.y = hit.point.y + playerHeight;
            transform.position = movePos;
        }

        MyInput();
        SpeedControl();
        MoveSpeedHandler();

        if (grounded)
            rb.drag = groundDrag;

        CheckGameOver();
    }

    void FixedUpdate()
    {
        MovePlayer();
        CheckSpritesToUpdate();
    }
    // private void LateUpdate() => CheckSpritesToUpdate();

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MoveSpeedHandler()
    {
        // state - Sprinting
        if (grounded && Input.GetKey(sprintKey))
            moveSpeed = sprintSpeed;

        // state - Walking
        else if (grounded)
            moveSpeed = walkSpeed;
    }

    void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
    }

    void SpeedControl()
    {
        // limiting speed on ground
        flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.CurrentSubState.OnTriggerEnter(other);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        playerHealth = Mathf.Clamp(playerHealth - damageAmount, 0, 100);
        UpdateHealthBar();

        if (PlayerConfigs.Instance.isMisha)
            PlayerConfigs.Instance.deathSFX[3].Play(transform.position);
        else
            PlayerConfigs.Instance.deathSFX[2].Play(transform.position);

        Debug.Log($"PALYER HEALTH REDUCED TO: {playerHealth}");
    }

    public void UpdateHealthBar() => PlayerConfigs.Instance.playerHealthBarImage.fillAmount = playerHealth / 100f;

    public void UpdatePowerBar()
    {
        PlayerConfigs.Instance.playerPowerBarImage.fillAmount = playerPower / 100f;

        if (playerPower >= 100)
            canUseUltimate = true;
        else
            canUseUltimate = false;
    }

    public void CheckGameOver()
    {
        if (playerHealth <= 0)
        {
            if (PlayerConfigs.Instance._livesRemaining <= 0)
            {
                agent.SetDestination(transform.position);
                StopPlayer();
                PlayerConfigs.Instance.IsGameOver = true;
                StartCoroutine(EndGame());

                //sfx
                if (PlayerConfigs.Instance.isMisha)
                    PlayerConfigs.Instance.deathSFX[1].Play(transform.position);
                else
                    PlayerConfigs.Instance.deathSFX[0].Play(transform.position);
                return;
            }

            if (isRespawning) return;

            StartCoroutine(RespawnDelay());

            agent.SetDestination(transform.position);
            StopPlayer();
            _currentState.ChangeAnimation("Death");
            Debug.Log("GAME OVER");
        }
    }

    public static void StopPlayer()
    {
        agent.acceleration = 0f;
        agent.enabled = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        _collider.enabled = false;
        _currentState.ChangeAnimation("Idle");
        _spriteRenderer.color = Color.grey;
    }


    public static void ResumePlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX |
                        RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _collider.enabled = true;
        _currentState.ChangeAnimation("Idle");
        _currentState.CheckAnimation();
        _spriteRenderer.color = Color.white;
    }

    IEnumerator RespawnDelay()
    {
        isRespawning = true;
        PlayerConfigs.Instance.RemoveHeartIcon();

        yield return new WaitForSeconds(1.7f);

        isRespawning = false;
        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        playerHealth = 100f;
        UpdateHealthBar();
        UpdatePowerBar();

        ResumePlayer();

        // reset to initial position
        // transform.SetPositionAndRotation(InitialPosition.Item1, InitialPosition.Item2);

        _currentState.ChangeAnimation("Idle");
        _currentState.CheckAnimation();
        Physics.SyncTransforms();

        agent.enabled = true;
        if (agent != null)
            agent.velocity = Vector3.zero;
        agent.acceleration = 0;
    }

    void CheckSpritesToUpdate()
    {
        foreach (SpritesLookAt _sprites in spritesLookAtList.Where(s => s != null))
        {
            if (Vector3.Distance(transform.position, _sprites.transform.position) < PlayerConfigs.Instance.spriteDistanceToUpdate)
                _sprites.DoUpdate = true;
            else
                _sprites.DoUpdate = false;
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.7f);
        LevelManager.Instance.RestartCurrentLevel();
    }

    #region PUZZLE

    public void AddKeyToInventory(string keyName) => KeysInventory.Add(keyName);

    public string GetKeyFromInventory(string keyName)
    {
        foreach (var key in KeysInventory)
        {
            if (key == keyName)
                return key;
        }
        return null;
    }

    public bool HasKey(string keyName) => KeysInventory.Contains(keyName);

    public void RemoveKeyFromInventory(string keyName) => KeysInventory.Remove(keyName);
    public void ClearKeyInventory() => KeysInventory.Clear();

    #endregion
}