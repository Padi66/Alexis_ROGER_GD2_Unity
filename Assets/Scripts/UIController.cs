using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private const int CARD_SPAWN_SCORE = 5;
    private const int GAME_OVER_SCORE = -1;
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScoreDatas _scoreDatas;
    [SerializeField] private GameObject _gameOverPanel;
    
    [Header("Access Card Spawn")]
    [SerializeField] private GameObject _accessCardPrefab;
    [SerializeField] private Transform _cardSpawnPoint;
    
    private bool _cardSpawned;
    private bool _gameOverAchieved;

    private void Start()
    {
        _scoreDatas.ScoreValue = 0;
        UpdateScore(0);
        
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Player_Collect.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        Player_Collect.OnScoreChanged -= UpdateScore;
    }
    
    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score : " + newScore.ToString();
        
        if (newScore >= CARD_SPAWN_SCORE && !_cardSpawned)
        {
            SpawnAccessCard();
        }
        else if (newScore <= GAME_OVER_SCORE && !_gameOverAchieved)
        {
            ShowGameOver();
        }
    }

    private void SpawnAccessCard()
    {
        _cardSpawned = true;
        
        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.DisableAllTargets();
        }
        
        if (_accessCardPrefab != null && _cardSpawnPoint != null)
        {
            Instantiate(_accessCardPrefab, _cardSpawnPoint.position, _cardSpawnPoint.rotation);
            Debug.Log("Carte d'accès apparue dans la scène ! Targets désactivées.");
        }
        else
        {
            Debug.LogWarning("AccessCardPrefab ou CardSpawnPoint non assigné dans UIController !");
        }
    }

    private void ShowGameOver()
    {
        _gameOverAchieved = true;
        
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.Log("GAME OVER ! Score tombé à -1 !");
        }
        
        Time.timeScale = 0f;
    }
}
