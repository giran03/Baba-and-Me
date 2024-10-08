using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    // interaction
    public bool IsInteracting { get; set; }

    // configs
    float _playerHealth;

    Rigidbody rb;
    Vector3 flatVel;
    (Vector3, Quaternion) initialPosition;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // public declarations
    public Vector3 moveDirection;

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
        initialPosition = (transform.position, transform.rotation);

        // set defaults
        _playerHealth = PlayerConfigs.Instance.playerHealth;
        KeysInventory = new();
    }

    void Update()
    {
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
        // PlayerBounds();

        if (grounded)
            rb.drag = groundDrag;
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

    void PlayerBounds()
    {
        if (transform.position.y < -20)
            RespawnPlayer();
    }

    void RespawnPlayer()
    {
        var (pos, rot) = initialPosition;
        transform.SetPositionAndRotation(pos, rot);
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        Physics.SyncTransforms();
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.CurrentSubState.OnTriggerEnter(other);
    }

    public void Damage(int damageAmount, float weaponCriticalDamage, float weaponCriticalChance)
    {
        _playerHealth -= damageAmount;
        Debug.Log($"PALYER HEALTH REDUCED TO: {_playerHealth}");
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