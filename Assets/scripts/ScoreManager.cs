using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int totalScore = 0;

    public void AddScore(int points)
    {
        totalScore += points;
        scoreText.text = "SCORE\n" + totalScore.ToString();
    }
}