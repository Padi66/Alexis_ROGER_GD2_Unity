using TMPro;
using UnityEngine;

// Contrôleur principal de l'interface utilisateur
// Gère l'affichage du score, le spawn de carte d'accès, et le game over
public class UIController : MonoBehaviour
{
    // Constantes pour les seuils de score
    // const = valeur fixe qui ne peut jamais changer (meilleure performance qu'une variable)
    private const int CARD_SPAWN_SCORE = 5;
    private const int GAME_OVER_SCORE = -1;
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _scoreText;
    
    // ScriptableObject qui stocke le score de manière persistante
    [SerializeField] private ScoreDatas _scoreDatas;
    
    // Panel UI à afficher lors du game over
    [SerializeField] private GameObject _gameOverPanel;
    
    [Header("Access Card Spawn")]
    // Prefab de la carte d'accès à spawner comme récompense
    [SerializeField] private GameObject _accessCardPrefab;
    
    // Point de spawn où la carte apparaîtra
    [SerializeField] private Transform _cardSpawnPoint;
    
    // Flag pour s'assurer que la carte ne spawn qu'une seule fois
    private bool _cardSpawned;
    
    // Flag pour s'assurer que le game over ne se déclenche qu'une seule fois
    private bool _gameOverAchieved;

    private void Start()
    {
        // Réinitialise le score à 0 au démarrage
        _scoreDatas.ScoreValue = 0;
        
        // Met à jour l'affichage avec le score initial
        UpdateScore(0);
        
        // Cache le panel de game over au démarrage
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);
    }

    // S'abonne à l'événement de changement de score
    private void OnEnable()
    {
        Player_Collect.OnScoreChanged += UpdateScore;
    }

    // Se désabonne de l'événement pour éviter les erreurs
    // CRITIQUE : évite d'appeler une méthode sur un objet détruit
    private void OnDisable()
    {
        Player_Collect.OnScoreChanged -= UpdateScore;
    }
    
    // Callback appelé automatiquement quand le score change via l'événement
    public void UpdateScore(int newScore)
    {
        // Mise à jour de l'affichage TextMeshPro
        _scoreText.text = "Score : " + newScore.ToString();
        
        // Vérifie si le joueur a atteint le seuil pour spawner la carte
        // && !_cardSpawned garantit que ça n'arrive qu'une seule fois
        if (newScore >= CARD_SPAWN_SCORE && !_cardSpawned)
        {
            SpawnAccessCard();
        }
        // else if : ne peut pas avoir à la fois carte ET game over
        // Vérifie si le score est tombé à -1 ou moins
        else if (newScore <= GAME_OVER_SCORE && !_gameOverAchieved)
        {
            ShowGameOver();
        }
    }

    // Spawne la carte d'accès comme récompense et désactive les cibles
    private void SpawnAccessCard()
    {
        // Marque immédiatement comme spawné pour éviter les duplications
        _cardSpawned = true;
        
        // Désactive toutes les cibles via le TargetManager (Singleton)
        // Empêche le joueur de continuer à collecter des cibles
        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.DisableAllTargets();
        }
        
        // Validation : vérifie que les références sont assignées
        if (_accessCardPrefab != null && _cardSpawnPoint != null)
        {
            // Instancie la carte au point de spawn configuré
            Instantiate(_accessCardPrefab, _cardSpawnPoint.position, _cardSpawnPoint.rotation);
            Debug.Log("Carte d'accès apparue dans la scène ! Targets désactivées.");
        }
        else
        {
            Debug.LogWarning("AccessCardPrefab ou CardSpawnPoint non assigné dans UIController !");
        }
    }

    // Affiche l'écran de game over et met le jeu en pause
    private void ShowGameOver()
    {
        // Marque immédiatement comme atteint pour éviter les duplications
        _gameOverAchieved = true;
        
        // Active le panel de game over s'il existe
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
        else
        {
            // Fallback si aucun panel n'est assigné
            Debug.Log("GAME OVER ! Score tombé à -1 !");
        }
        
        // Time.timeScale = 0 met le jeu en pause
        // 0 = pause totale, 1 = vitesse normale, 2 = vitesse x2.
        // Affecte Update(), FixedUpdate(), animations, physics, mais PAS OnGUI() ni les coroutines avec WaitForSecondsRealtime
        Time.timeScale = 0f;
    }
}
