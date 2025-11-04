using System.Collections;
using UnityEngine;

public class TargetSoft : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int _targetValue = -1;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private float _currentDirection;
    private bool _wasCollected = false;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        
        if (_collider != null)
        {
            _collider.isTrigger = false;
        }
        
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        
        StartCoroutine(IgnoreOtherTargetsDelayed());
        ChooseRandomDirection();
    }
    
    private IEnumerator IgnoreOtherTargetsDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        
        TargetSoft[] allTargetSofts = FindObjectsByType<TargetSoft>(FindObjectsSortMode.None);
        foreach (TargetSoft otherTarget in allTargetSofts)
        {
            if (otherTarget != this && otherTarget != null)
            {
                Collider otherCollider = otherTarget.GetComponent<Collider>();
                if (otherCollider != null && _collider != null)
                {
                    Physics.IgnoreCollision(_collider, otherCollider);
                }
            }
        }
        
        TargetHard[] allTargetHards = FindObjectsByType<TargetHard>(FindObjectsSortMode.None);
        foreach (TargetHard otherTarget in allTargetHards)
        {
            if (otherTarget != null)
            {
                Collider otherCollider = otherTarget.GetComponent<Collider>();
                if (otherCollider != null && _collider != null)
                {
                    Physics.IgnoreCollision(_collider, otherCollider);
                }
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (!_wasCollected && _rigidbody != null)
        {
            Vector3 movement = new Vector3(_currentDirection * _moveSpeed, 0, 0);
            _rigidbody.linearVelocity = movement;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (_wasCollected) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            _wasCollected = true;
            
            Player_Collect playerCollect = other.gameObject.GetComponent<Player_Collect>();
            if (playerCollect != null)
            {
                playerCollect.UpdateScoreWithoutEvent(_targetValue);
                
                HideTarget();
                
                if (TargetManager.Instance != null)
                {
                    TargetManager.Instance.OnTargetCollected(gameObject);
                }
            }
        }
        else if (IsWall(other.gameObject))
        {
            _currentDirection *= -1;
        }
    }
    
    private void HideTarget()
    {
        if (_meshRenderer != null)
            _meshRenderer.enabled = false;
        if (_collider != null)
            _collider.enabled = false;
        if (_rigidbody != null)
            _rigidbody.linearVelocity = Vector3.zero;
    }
    
    private bool IsWall(GameObject obj)
    {
        return obj.name.Contains("Wall") || obj.name.Contains("Ground");
    }
    
    private void ChooseRandomDirection()
    {
        _currentDirection = UnityEngine.Random.value < 0.5f ? -1f : 1f;
    }
}
