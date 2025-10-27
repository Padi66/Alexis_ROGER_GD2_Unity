using UnityEngine;

public class AccessCard : MonoBehaviour
{
    [SerializeField] private AccessCardData _cardData;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private bool _destroyOnCollect = true;
    [SerializeField] private AudioClip _collectSound;
    
    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        if (_cardData != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = _cardData.cardColor;
            }
        }
    }
    
    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCard(other.gameObject);
        }
    }
    
    private void CollectCard(GameObject player)
    {
        if (InventoryManager.Instance != null && _cardData != null)
        {
            InventoryManager.Instance.AddCard(_cardData);
            
            if (_collectSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_collectSound);
            }
            
            if (_destroyOnCollect)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
