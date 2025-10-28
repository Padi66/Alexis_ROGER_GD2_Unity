using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _grappinDirection;
    private Vector3 _grappinHit; 
    
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 2.0f;
    
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;
    
    private Vector3 _movement;
    private bool _isGrounded;
    private bool _jumpRequested;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _verticalMovement = Input.GetAxis("Vertical");
        
        if (Input.GetButtonDown("Jump"))
        {
            _jumpRequested = true;
        }
        
        CheckGroundStatus();
    }
    
    void FixedUpdate()
    {
        _movement = new Vector3(_horizontalMovement, 0f, _verticalMovement);
        _movement.Normalize();
        _movement *= _speed;
        _movement.y = _rb.linearVelocity.y;
    
        if (_rb != null)
        {
            _rb.linearVelocity = _movement;
        
            if (_jumpRequested && _isGrounded)
            {
                Jump();
            }
        
            ApplyFallGravity();
        
            _jumpRequested = false;
        }
        else
        {
            Debug.LogError("No RigidBody found !");
        }
    }

    
    private void Jump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    
    private void CheckGroundStatus()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance + 0.5f, _groundLayer);
    }
    private void ApplyFallGravity()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public float GetJumpForce()
    {
        return _jumpForce;
    }

    public void SetJumpForce(float jumpForce)
    {
        _jumpForce = jumpForce;
    }
}
