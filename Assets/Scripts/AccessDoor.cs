using UnityEngine;

public class AccessDoor : MonoBehaviour
{
    [SerializeField] private AccessCardData _requiredCard;
    [SerializeField] private GameObject _doorVisual;
    [SerializeField] private bool _consumeCard = false;
    [SerializeField] private Color _lockedColor = Color.red;
    [SerializeField] private Color _unlockedColor = Color.green;
    [SerializeField] private AudioClip _unlockSound;
    [SerializeField] private AudioClip _deniedSound;
    
    private Renderer _renderer;
    private AudioSource _audioSource;
    private bool _isUnlocked = false;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
        
        UpdateVisual();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isUnlocked)
        {
            TryUnlock();
        }
    }
    
    private void TryUnlock()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasCard(_requiredCard))
        {
            _isUnlocked = true;
            
            if (_consumeCard)
            {
                InventoryManager.Instance.RemoveCard(_requiredCard);
            }
            
            if (_unlockSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_unlockSound);
            }
            
            OpenDoor();
        }
        else
        {
            if (_deniedSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_deniedSound);
            }
            
            Debug.Log($"Carte d'accès requise: {_requiredCard?.cardName ?? "Non définie"}");
        }
    }
    
    private void OpenDoor()
    {
        UpdateVisual();
        
        if (_doorVisual != null)
        {
            _doorVisual.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    private void UpdateVisual()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _isUnlocked ? _unlockedColor : _lockedColor;
        }
    }
}
