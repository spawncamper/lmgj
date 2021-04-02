using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public delegate void ScoreChanged();
    public static event ScoreChanged ScoreChangedEvent;

    int totalScore = 0;
    
    void AddToScore()
    {
        totalScore++;

        if (ScoreChangedEvent != null)
            ScoreChangedEvent();
    }

    public int GetScore()
    {
        return totalScore;
    }

    public void ResetScore()
    {
        totalScore = 0;
    }
}
