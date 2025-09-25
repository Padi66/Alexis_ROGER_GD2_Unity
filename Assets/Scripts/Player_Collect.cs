using UnityEngine;

public class Player_Collect : MonoBehaviour
{
    [SerializeField] private ScoreDatas _scoreDatas;
    [SerializeField] private UIController _uiController;

    public void UpdateScore(int value)
    {
        _scoreDatas.ScoreValue = Mathf.Clamp(_scoreDatas.ScoreValue + value,min:0, max:_scoreDatas.ScoreValue + value);
        _uiController.UpdateScore(_scoreDatas.ScoreValue);
    }
}
