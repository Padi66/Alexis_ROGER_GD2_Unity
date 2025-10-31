using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private const int VICTORY_SCORE = 10;
    private const int GAME_OVER_SCORE = -1;
    
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScoreDatas _scoreDatas;
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _gameOverPanel;
    
    private bool _victoryAchieved;
    private bool _gameOverAchieved;

    private void Start()
    {
        _scoreDatas.ScoreValue = 0;
        UpdateScore(0);
        
        if (_victoryPanel != null)
            _victoryPanel.SetActive(false);
        
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
        
        if (newScore >= VICTORY_SCORE && !_victoryAchieved)
        {
            ShowVictory();
        }
        else if (newScore <= GAME_OVER_SCORE && !_gameOverAchieved)
        {
            ShowGameOver();
        }
    }

    private void ShowVictory()
    {
        _victoryAchieved = true;
        
        if (_victoryPanel != null)
        {
            _victoryPanel.SetActive(true);
        }
        
        Time.timeScale = 0f;
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

