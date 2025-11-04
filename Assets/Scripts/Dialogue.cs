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
    
    public void StopCurrentDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
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
        
        StopCurrentDialogue();
        
        dialoguePanel.SetActive(true);
        Debug.Log("Dialogue: Panel activé");
        
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    
    private IEnumerator TypeText(string text)
    {
        Debug.Log("Dialogue: Début de l'effet lettre par lettre");
        dialogueText.text = "";
        
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
        
        Debug.Log("Dialogue: Effet lettre par lettre terminé");
    }
    
    public void HideDialogue()
    {
        StopCurrentDialogue();
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            Debug.Log("Dialogue: Panel caché");
        }
    }
}
