using UnityEngine;

// Zone de trigger qui déclenche l'écran de victoire quand le joueur entre
public class VictoryZone : MonoBehaviour
{
    [Header("Victory Settings")]
    // Panel UI à afficher lors de la victoire
    [SerializeField] private GameObject _victoryPanel;
    
    // Flag pour empêcher de déclencher la victoire plusieurs fois
    // Protège contre les multiples entrées dans le trigger
    private bool _victoryTriggered = false;

    private void Start()
    {
        // Cache le panel de victoire au démarrage
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Guard clause : sort immédiatement si la victoire a déjà été déclenchée
        // Évite de traiter plusieurs fois le trigger
        if (_victoryTriggered) return;
        
        // Déclenche la victoire dès que le joueur entre dans la zone
        if (other.CompareTag("Player"))
        {
            TriggerVictory();
        }
    }

    // Méthode qui gère toutes les conséquences de la victoire
    private void TriggerVictory()
    {
        // Marque immédiatement comme déclenché pour éviter les duplications
        _victoryTriggered = true;
        
        // Affiche le panel de victoire s'il est assigné
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(true);
            Debug.Log("VictoryZone: Victoire !");
        }
        else
        {
            // Avertissement si le panel n'est pas configuré
            Debug.LogWarning("VictoryZone: Victory Panel non assigné !");
        }
        
        // Time.timeScale = 0 met le jeu en pause
        // Fige le temps du jeu : Update(), FixedUpdate(), animations, physics s'arrêtent
        // Les UI et les coroutines avec WaitForSecondsRealtime continuent de fonctionner
        Time.timeScale = 0f;
    }
}