using UnityEngine;
using System.Collections;
public class TriggerDialogue : MonoBehaviour
{
[Header("Messages")]
    [TextArea(3, 5)]
    public string[] messages = new string[] 
    { 
        "Premier message...", 
        "Deuxième message qui suit..." 
    };
    
    [Header("Settings")]
    public float delayBetweenMessages = 3f;
    public bool showOnce = true;
    public float finalDisplayDuration = 5f;
    
    private bool hasShown = false;
    private const string PLAYER_TAG = "Player";
    private Dialogue dialogue;
    private Coroutine sequenceCoroutine;
    
    private static TriggerDialogue currentActiveTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if (showOnce && hasShown)
            {
                return;
            }
            
            dialogue = FindFirstObjectByType<Dialogue>();
            
            if (dialogue != null)
            {
                if (currentActiveTrigger != null && currentActiveTrigger != this)
                {
                    currentActiveTrigger.StopSequence();
                }
                
                currentActiveTrigger = this;
                
                dialogue.StopCurrentDialogue();
                
                sequenceCoroutine = StartCoroutine(ShowMessagesSequence());
                hasShown = true;
            }
            else
            {
                Debug.LogError("TriggerDialogue: Aucun script Dialogue trouvé!");
            }
        }
    }
    
    private void StopSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }
    }
    
    private IEnumerator ShowMessagesSequence()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            if (currentActiveTrigger != this)
            {
                yield break;
            }
            
            dialogue.ShowDialogue(messages[i]);
            
            if (i < messages.Length - 1)
            {
                yield return new WaitForSeconds(delayBetweenMessages);
            }
        }
        
        if (finalDisplayDuration > 0)
        {
            yield return new WaitForSeconds(finalDisplayDuration);
            
            if (currentActiveTrigger == this)
            {
                dialogue.HideDialogue();
            }
        }
    }
}
