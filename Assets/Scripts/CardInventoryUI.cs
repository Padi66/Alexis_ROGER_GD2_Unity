using UnityEngine;
using TMPro;
public class CardInventoryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _cardCountText;
    [SerializeField] private string _textFormat = "Cartes: {0}";
    
    private void OnEnable()
    {
        InventoryManager.OnCardCollected += UpdateCardDisplay;
        InventoryManager.OnCardRemoved += UpdateCardDisplay;
    }
    
    private void OnDisable()
    {
        InventoryManager.OnCardCollected -= UpdateCardDisplay;
        InventoryManager.OnCardRemoved -= UpdateCardDisplay;
    }
    
    private void Start()
    {
        UpdateCardDisplay(null);
    }
    
    private void UpdateCardDisplay(AccessCardData card)
    {
        if (_cardCountText != null && InventoryManager.Instance != null)
        {
            int cardCount = InventoryManager.Instance.GetCardCount();
            _cardCountText.text = string.Format(_textFormat, cardCount);
        }
    }
}
