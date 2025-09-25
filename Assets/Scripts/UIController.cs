using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private void OnEnable()
    {
        Player_Collect.OnTargetColleted += UpdateScore;
    }

    private void OnDisable()
    {
        Player_Collect.OnTargetColleted -= UpdateScore;
    }
    
    
    private void Star()
    {
        UpdateScore(0);
    }
    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score : " + newScore.ToString();
        //_scoreText.text = $"Score : {newScore.ToString()}";
        
    }
    
}
