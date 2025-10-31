using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private DisappearingWall[] _wallsToControl;
    [SerializeField] private float _cooldown = 1f;
    [SerializeField] private Material _activatedMaterial;
    [SerializeField] private Material _defaultMaterial;
    
    private MeshRenderer _meshRenderer;
    private float _lastActivationTime;
    private bool _isActivated;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        
        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isActivated)
        {
            if (Time.time - _lastActivationTime >= _cooldown)
            {
                ActivatePlate();
            }
        }
    }

    private void ActivatePlate()
    {
        _lastActivationTime = Time.time;
        _isActivated = true;
        
        if (_meshRenderer != null && _activatedMaterial != null)
        {
            _meshRenderer.material = _activatedMaterial;
        }
        
        foreach (DisappearingWall wall in _wallsToControl)
        {
            if (wall != null)
            {
                wall.Disappear();
            }
        }
        
        Debug.Log($"{gameObject.name} activated!");
        
        Invoke(nameof(ResetPlate), _cooldown);
    }

    private void ResetPlate()
    {
        _isActivated = false;
        
        if (_meshRenderer != null && _defaultMaterial != null)
        {
            _meshRenderer.material = _defaultMaterial;
        }
    }
}
