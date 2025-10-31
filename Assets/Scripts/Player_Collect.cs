using System;
using UnityEngine;

public class Player_Collect : MonoBehaviour
{
    [SerializeField] private ScoreDatas _scoreDatas;
    [SerializeField] private UIController _uiController;

    public static Action<int> OnTargetColleted;
    public static Action<int> OnScoreChanged;
        
    public void UpdateScore(int value)
    {
        UpdateScoreInternal(value);
        OnTargetColleted?.Invoke(_scoreDatas.ScoreValue);
        OnScoreChanged?.Invoke(_scoreDatas.ScoreValue);
    }

    public void UpdateScoreWithoutEvent(int value)
    {
        UpdateScoreInternal(value);
        OnScoreChanged?.Invoke(_scoreDatas.ScoreValue);
    }

    private void UpdateScoreInternal(int value)
    {
        _scoreDatas.ScoreValue += value;
    }
}
