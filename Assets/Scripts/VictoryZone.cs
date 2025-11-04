using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    [Header("Victory Settings")]
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private bool _requiresAccessCard = false;
    [SerializeField] private AccessCardData _requiredCard;
    
    private bool _victoryTriggered = false;

    private void Start()
    {
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_victoryTriggered) return;
        
        if (other.CompareTag("Player"))
        {
            if (_requiresAccessCard)
            {
                if (InventoryManager.Instance != null && _requiredCard != null)
                {
                    if (InventoryManager.Instance.HasCard(_requiredCard))
                    {
                        TriggerVictory();
                    }
                    else
                    {
                        Debug.Log("VictoryZone: Vous avez besoin de la carte d'accès !");
                    }
                }
            }
            else
            {
                TriggerVictory();
            }
        }
    }

    private void TriggerVictory()
    {
        _victoryTriggered = true;
        
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(true);
            Debug.Log("VictoryZone: Victoire !");
        }
        else
        {
            Debug.LogWarning("VictoryZone: Victory Panel non assigné !");
        }
        
        Time.timeScale = 0f;
    }
}