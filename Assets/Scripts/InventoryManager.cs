using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    private HashSet<AccessCardData> _collectedCards = new HashSet<AccessCardData>();
    
    // Déclaration d'un type de délégué pour les événements de collecte
    // Définit la signature des méthodes qui peuvent s'abonner à cet événement
    public delegate void CardCollected(AccessCardData card);
    
    // Événement statique : permet à d'autres scripts de s'abonner et d'être notifiés
    // quand une carte est collectée, sans créer de dépendances directes
    public static event CardCollected OnCardCollected;
    public delegate void CardRemoved(AccessCardData card);
    public static event CardRemoved OnCardRemoved;
    
    // Awake s'exécute avant Start
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            // Empêche la destruction de cet objet lors du changement de scène
            // Permet de conserver l'inventaire entre les différents niveaux
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si une instance existe déjà, détruit ce doublon
            // Garantit qu'il n'y a qu'un seul InventoryManager dans le jeu
            Destroy(gameObject);
        }
    }
    
    public void AddCard(AccessCardData card)
    {
        // .Contains() est très rapide
        if (card != null && !_collectedCards.Contains(card))
        {
            _collectedCards.Add(card);
            
            // Opérateur null-conditionnel (?.) : invoque l'événement seulement s'il a des abonnés
            // Évite une NullReferenceException si aucun script n'écoute l'événement
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
            
            // Notifie tous les scripts abonnés qu'une carte a été retirée
            OnCardRemoved?.Invoke(card);
            Debug.Log($"Carte d'accès utilisée: {card.cardName}");
        }
    }
    
    public void ClearInventory()
    {
        _collectedCards.Clear();
    }
}
