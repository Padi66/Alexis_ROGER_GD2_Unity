using System.Collections;
using UnityEngine;

public class AccessCard : MonoBehaviour
{
    [SerializeField] private AccessCardData _cardData;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private bool _destroyOnCollect = true;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _collectSound;
    [SerializeField] [Range(0f, 3f)] private float _soundVolume = 1.5f;
    
    private void Start()
    {
        if (_cardData != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = _cardData.cardColor;
            }
        }
        
        StartCoroutine(IgnoreTargetsDelayed());
    }
    
    private IEnumerator IgnoreTargetsDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        
        Collider cardCollider = GetComponent<Collider>();
        if (cardCollider == null) yield break;
        
        TargetHard[] allTargetHards = FindObjectsByType<TargetHard>(FindObjectsSortMode.None);
        foreach (TargetHard target in allTargetHards)
        {
            if (target != null)
            {
                Collider targetCollider = target.GetComponent<Collider>();
                if (targetCollider != null)
                {
                    Physics.IgnoreCollision(cardCollider, targetCollider);
                }
            }
        }
        
        TargetSoft[] allTargetSofts = FindObjectsByType<TargetSoft>(FindObjectsSortMode.None);
        foreach (TargetSoft target in allTargetSofts)
        {
            if (target != null)
            {
                Collider targetCollider = target.GetComponent<Collider>();
                if (targetCollider != null)
                {
                    Physics.IgnoreCollision(cardCollider, targetCollider);
                }
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
            
            if (_collectSound != null)
            {
                AudioSource.PlayClipAtPoint(_collectSound, transform.position, _soundVolume);
                Debug.Log($"AccessCard: Playing sound at volume {_soundVolume}");
            }
            else
            {
                Debug.LogWarning("AccessCard: Collect Sound not assigned!");
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
