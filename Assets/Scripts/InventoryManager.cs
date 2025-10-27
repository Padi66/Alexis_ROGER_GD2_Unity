using UnityEngine;
using System.Collections.Generic;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    private HashSet<AccessCardData> _collectedCards = new HashSet<AccessCardData>();
    
    public delegate void CardCollected(AccessCardData card);
    public static event CardCollected OnCardCollected;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddCard(AccessCardData card)
    {
        if (card != null && !_collectedCards.Contains(card))
        {
            _collectedCards.Add(card);
            OnCardCollected?.Invoke(card);
            Debug.Log($"Carte d'accès collectée: {card.cardName}");
        }
    }
    
    public bool HasCard(AccessCardData card)
    {
        return _collectedCards.Contains(card);
    }
    
    public int GetCardCount()
    {
        return _collectedCards.Count;
    }
    
    public void RemoveCard(AccessCardData card)
    {
        if (_collectedCards.Contains(card))
        {
            _collectedCards.Remove(card);
        }
    }
    
    public void ClearInventory()
    {
        _collectedCards.Clear();
    }
}
