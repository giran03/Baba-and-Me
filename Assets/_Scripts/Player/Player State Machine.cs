using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    float _playerHealth;

    Rigidbody rb;
    Vector3 flatVel;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // respawn
    (Vector3, quaternion) InitialPosition;

    // public declarations
    public Vector3 moveDirection;
    private bool isRespawning;

    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }

    private void Awake()
    {
        _states = new(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        InitialPosition = (transform.position, transform.rotation);

        // set defaults
        _playerHealth = PlayerConfigs.Instance.playerHealth;
        KeysInventory = new();

        Time.timeScale = 1;
    }

    void Update()
    {
        if (PlayerConfigs.Instance.IsGameOver) return;
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

    void FixedUpdate() => MovePlayer();

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
        _playerHealth -= damageAmount;
        UpdateHealthBar();

        Debug.Log($"PALYER HEALTH REDUCED TO: {_playerHealth}");
    }

    void UpdateHealthBar()
    {
        PlayerConfigs.Instance.playerHealthBarImage.fillAmount = _playerHealth / 100f; //player max health = 100f
    }

    public void CheckGameOver()
    {
        if (_playerHealth <= 0)
        {
            // PlayerConfigs.Instance.IsGameOver = true;
            if (isRespawning) return;

            StartCoroutine(RespawnDelay());

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<NavMeshAgent>().enabled = false;
            _currentState.ChangeAnimation("Death");
            Debug.Log("GAME OVER");
        }
    }

    IEnumerator RespawnDelay()
    {
        isRespawning = true;

        yield return new WaitForSeconds(1.7f);

        isRespawning = false;
        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        _playerHealth = 100f;
        UpdateHealthBar();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
        transform.SetPositionAndRotation(InitialPosition.Item1, InitialPosition.Item2);
        _currentState.ChangeAnimation("Idle");
        Physics.SyncTransforms();

        GetComponent<NavMeshAgent>().enabled = true;
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