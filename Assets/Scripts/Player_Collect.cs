using UnityEngine;

public class Player_Collect : MonoBehaviour
{
    [SerializeField] private ScoreDatas _scoreDatas;

    public void UpdateScore(int value)
    {
        _scoreDatas.ScoreValue = Mathf.Clamp(_scoreDatas.ScoreValue + value,min:0, max:_scoreDatas.ScoreValue + value);
    }
}
