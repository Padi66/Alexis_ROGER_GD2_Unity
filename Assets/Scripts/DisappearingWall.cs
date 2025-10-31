using UnityEngine;
using System.Collections;
using UnityEngine;

public class DisappearingWall : MonoBehaviour
{
    [SerializeField] private float _disappearDuration = 5f;
    
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private bool _isDisappeared;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void Disappear()
    {
        if (_isDisappeared) return;
        
        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        _isDisappeared = true;
        
        if (_meshRenderer != null)
            _meshRenderer.enabled = false;
        
        if (_collider != null)
            _collider.enabled = false;
        
        Debug.Log($"{gameObject.name} disappeared for {_disappearDuration} seconds");
        
        yield return new WaitForSeconds(_disappearDuration);
        
        if (_meshRenderer != null)
            _meshRenderer.enabled = true;
        
        if (_collider != null)
            _collider.enabled = true;
        
        Debug.Log($"{gameObject.name} reappeared");
        _isDisappeared = false;
    }
}