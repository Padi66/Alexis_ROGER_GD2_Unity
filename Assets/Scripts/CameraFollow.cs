using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private GameObject _player;
    
    [Header("Follow Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private bool _lookAtPlayer = true;
    
    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 2f;
    [SerializeField] private float _minZoom = 3f;
    [SerializeField] private float _maxZoom = 15f;
    
    private Vector3 _currentOffset;
    
    private void Start()
    {
        _currentOffset = _offset.normalized * _minZoom;
    }
    
    private void LateUpdate()
    {
        if (_player == null)
        {
            Debug.LogError("Player reference is missing in CameraFollow!");
            return;
        }
        
        HandleZoom();
        
        Vector3 desiredPosition = _player.transform.position + _currentOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
        
        if (_lookAtPlayer)
        {
            transform.LookAt(_player.transform);
        }
    }
    
    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        if (scrollInput != 0f)
        {
            float zoomAmount = scrollInput * _zoomSpeed;
            Vector3 zoomDirection = _currentOffset.normalized;
            Vector3 newOffset = _currentOffset + zoomDirection * zoomAmount;
            
            float newDistance = newOffset.magnitude;
            
            if (newDistance >= _minZoom && newDistance <= _maxZoom)
            {
                _currentOffset = newOffset;
            }
            else
            {
                _currentOffset = zoomDirection * Mathf.Clamp(newDistance, _minZoom, _maxZoom);
            }
        }
    }
}