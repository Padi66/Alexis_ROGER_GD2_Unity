using System;
using System.Collections;
using UnityEngine;

public class TargetSoft : MonoBehaviour
{
    [SerializeField] private int _targetValue = 1;
    [SerializeField] private float _shadowDuration = 3f;
    [SerializeField] private GameObject _particleEffect;
    
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private bool _canCollect = true;
    private Coroutine _respawnCoroutine;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        
        if (_collider != null && !_collider.isTrigger)
        {
            _collider.isTrigger = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_canCollect) return;
        
        if (other.CompareTag("Player"))
        {
            Player_Collect playerCollect = other.GetComponent<Player_Collect>();
            if (playerCollect != null)
            {
                playerCollect.UpdateScore(_targetValue);
                
                if (_respawnCoroutine != null)
                {
                    StopCoroutine(_respawnCoroutine);
                }
                
                _respawnCoroutine = StartCoroutine(ShadowTimerControl());
            }
        }
    }

    private void ToggleVisibility(bool newVisibility)
    {
        _canCollect = newVisibility;
        
        if (_meshRenderer != null)
        {
            _meshRenderer.enabled = newVisibility;
        }
        
        if (_collider != null)
        {
            _collider.enabled = newVisibility;
        }
    }
    
    private IEnumerator ShadowTimerControl()
    {
        ToggleVisibility(false);
        yield return new WaitForSeconds(_shadowDuration);
        ToggleVisibility(true);
        _respawnCoroutine = null;
    }
       
}
