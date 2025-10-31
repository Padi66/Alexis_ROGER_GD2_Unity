using System.Collections;
using UnityEngine;

public class TargetHard : MonoBehaviour
{
    private const float RESPAWN_DELAY = 3f;
    
    [SerializeField] private int _targetValue = 1;
    
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _isRespawning;
    private MeshRenderer _meshRenderer;
    private Collider _collider;

    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        
        Debug.Log($"TargetHard initialized at position: {_initialPosition}");
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"TargetHard collision with: {other.gameObject.name}, IsRespawning: {_isRespawning}");
        
        if (_isRespawning) return;
        
        Player_Collect playerCollect = other.gameObject.GetComponent<Player_Collect>();
        if (playerCollect != null)
        {
            Debug.Log($"Player found! Updating score by {_targetValue}");
            playerCollect.UpdateScoreWithoutEvent(_targetValue);
            StartCoroutine(RespawnRoutine());
        }
        else
        {
            Debug.LogWarning($"Player_Collect component not found on {other.gameObject.name}");
        }
    }

    private IEnumerator RespawnRoutine()
    {
        _isRespawning = true;
        Debug.Log("TargetHard hiding...");
        
        if (_meshRenderer != null)
            _meshRenderer.enabled = false;
        if (_collider != null)
            _collider.enabled = false;
        
        yield return new WaitForSeconds(RESPAWN_DELAY);
        
        Debug.Log("TargetHard respawning...");
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        
        if (_meshRenderer != null)
            _meshRenderer.enabled = true;
        if (_collider != null)
            _collider.enabled = true;
            
        _isRespawning = false;
    }
}
