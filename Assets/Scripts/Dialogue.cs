using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
[Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [Header("Settings")]
    [SerializeField] private float letterDelay = 0.05f;
    
    // Garde une référence à la coroutine en cours pour pouvoir l'arrêter si nécessaire
    private Coroutine typingCoroutine;
    
    private void Start()
    {
        if (dialoguePanel == null)
        {
            Debug.LogError("Dialogue: ERREUR - DialoguePanel n'est pas assigné!");
        }
        
        if (dialogueText == null)
        {
            Debug.LogError("Dialogue: ERREUR - DialogueText n'est pas assigné!");
        }
        
        HideDialogue();
        Debug.Log("Dialogue: Script initialisé.");
    }
    
    // Arrête proprement le dialogue en cours pour éviter les conflits
    public void StopCurrentDialogue()
    {
        // Arrête la coroutine d'effet de frappe si elle est active
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        // Annule tous les Invoke en attente sur ce MonoBehaviour
        CancelInvoke();
    }
    
    public void ShowDialogue(string text)
    {
        Debug.Log($"Dialogue: ShowDialogue appelé avec le texte: {text}");
        
        if (dialoguePanel == null)
        {
            Debug.LogError("Dialogue: Impossible d'afficher - DialoguePanel est null!");
            return;
        }
        
        if (dialogueText == null)
        {
            Debug.LogError("Dialogue: Impossible d'afficher - DialogueText est null!");
            return;
        }
        
        // Important : arrête le dialogue précédent avant d'en démarrer un nouveau
        // Cela évite d'avoir plusieurs coroutines qui affichent du texte en même temps
        StopCurrentDialogue();
        
        dialoguePanel.SetActive(true);
        Debug.Log("Dialogue: Panel activé");
        
        // Lance la coroutine et garde sa référence pour pouvoir l'arrêter plus tard
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    
    // Coroutine qui affiche le texte lettre par lettre pour créer un effet de frappe
    private IEnumerator TypeText(string text)
    {
        Debug.Log("Dialogue: Début de l'effet lettre par lettre");
        dialogueText.text = "";
        
        // Parcourt chaque caractère du texte un par un
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            // Attend un délai avant d'afficher la prochaine lettre
            yield return new WaitForSeconds(letterDelay);
        }
        
        Debug.Log("Dialogue: Effet lettre par lettre terminé");
    }
    
    public void HideDialogue()
    {
        // Arrête d'abord le dialogue en cours avant de masquer le panel
        StopCurrentDialogue();
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            Debug.Log("Dialogue: Panel caché");
        }
    }
}
