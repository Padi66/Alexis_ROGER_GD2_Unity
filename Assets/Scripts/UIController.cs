using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScoreDatas _scoreDatas;

    private void Start()
    {
        _scoreDatas.ScoreValue = 0;
        UpdateScore(0);
    }

    private void OnEnable()
    {
        Player_Collect.OnTargetColleted += UpdateScore;
    }

    private void OnDisable()
    {
        Player_Collect.OnTargetColleted -= UpdateScore;
    }
    
    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score : " + newScore.ToString();
    }
}

