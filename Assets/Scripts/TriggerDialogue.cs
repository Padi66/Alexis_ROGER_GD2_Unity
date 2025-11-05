using UnityEngine;
using System.Collections;

// Zone de trigger qui affiche une séquence de messages de dialogue quand le joueur entre
// Gère l'affichage progressif de plusieurs messages avec délais
public class TriggerDialogue : MonoBehaviour
{
    [Header("Messages")]
    [TextArea(3, 5)]
    public string[] messages = new string[] 
    { 
        "Premier message...", 
        "Deuxième message..." 
    };
    
    [Header("Settings")]
    // Délai d'attente entre chaque message de la séquence
    public float delayBetweenMessages = 3f;
    
    // Si vrai, le dialogue ne s'affiche qu'une seule fois
    public bool showOnce = true;
    
    // Durée d'affichage du dernier message avant de masquer le dialogue
    public float finalDisplayDuration = 5f;
    
    // Flag pour savoir si ce trigger a déjà été activé
    private bool hasShown = false;
    
    // Constante pour éviter les erreurs de frappe dans le tag
    private const string PLAYER_TAG = "Player";
    
    // Référence au système de dialogue
    private Dialogue dialogue;
    
    // Stocke la référence de la coroutine en cours pour pouvoir l'arrêter
    private Coroutine sequenceCoroutine;
    
    // Variable STATIQUE partagée entre tous les TriggerDialogue
    // Permet de s'assurer qu'un seul trigger est actif à la fois
    private static TriggerDialogue currentActiveTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            // Si configuré pour une seule utilisation et déjà montré, sort immédiatement
            if (showOnce && hasShown)
            {
                return;
            }
            
            // Trouve le système de dialogue dans la scène
            dialogue = FindFirstObjectByType<Dialogue>();
            
            if (dialogue != null)
            {
                // Si un autre trigger est déjà actif, l'arrête d'abord
                // Évite les conflits entre plusieurs zones de dialogue
                if (currentActiveTrigger != null && currentActiveTrigger != this)
                {
                    currentActiveTrigger.StopSequence();
                }
                
                // Enregistre ce trigger comme étant actif globalement
                currentActiveTrigger = this;
                
                // Arrête tout dialogue en cours avant de démarrer le nouveau
                dialogue.StopCurrentDialogue();
                
                // Lance la séquence de messages et stocke la référence de la coroutine
                sequenceCoroutine = StartCoroutine(ShowMessagesSequence());
                
                // Marque comme déjà affiché
                hasShown = true;
            }
            else
            {
                Debug.LogError("TriggerDialogue: Aucun script Dialogue trouvé!");
            }
        }
    }
    
    // Arrête proprement la séquence de messages en cours
    private void StopSequence()
    {
        if (sequenceCoroutine != null)
        {
            // Arrête la coroutine
            StopCoroutine(sequenceCoroutine);
            
            // Nettoie la référence pour libérer la mémoire
            sequenceCoroutine = null;
        }
    }
    
    // Coroutine qui affiche les messages un par un avec des délais
    private IEnumerator ShowMessagesSequence()
    {
        // Boucle à travers tous les messages du tableau
        for (int i = 0; i < messages.Length; i++)
        {
            // Vérifie si ce trigger est toujours le trigger actif
            // Si un autre trigger a pris le contrôle, arrête cette séquence
            if (currentActiveTrigger != this)
            {
                yield break; // Sort de la coroutine immédiatement
            }
            
            // Affiche le message actuel
            dialogue.ShowDialogue(messages[i]);
            
            // Attend avant le prochain message, SAUF pour le dernier
            // i < messages.Length - 1 signifie "tous sauf le dernier"
            if (i < messages.Length - 1)
            {
                // Pause de X secondes avant le message suivant
                yield return new WaitForSeconds(delayBetweenMessages);
            }
        }
        
        // Après le dernier message, attend avant de masquer le dialogue
        if (finalDisplayDuration > 0)
        {
            yield return new WaitForSeconds(finalDisplayDuration);
            
            // Vérifie encore que ce trigger est toujours actif avant de masquer
            // Évite de masquer si un autre dialogue a pris le contrôle
            if (currentActiveTrigger == this)
            {
                dialogue.HideDialogue();
            }
        }
    }
}
