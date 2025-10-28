using UnityEngine;

public class BoostZone : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private float _speedMultiplier = 2.0f;
    [SerializeField] private float _jumpMultiplier = 1.5f;
    [SerializeField] private float _boostDuration = 5.0f;
    
    [Header("Visual Feedback")]
    [SerializeField] private Color _boostColor = Color.yellow;
    [SerializeField] private AudioClip _boostSound;
    
    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = _boostColor;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBoost playerBoost = other.GetComponent<PlayerBoost>();
            if (playerBoost != null)
            {
                playerBoost.ApplyBoost(_speedMultiplier, _jumpMultiplier, _boostDuration);
                
                if (_boostSound != null && _audioSource != null)
                {
                    _audioSource.PlayOneShot(_boostSound);
                }
            }
        }
    }
}
