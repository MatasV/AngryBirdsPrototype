using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private ScoreDatabase scoreDatabase;

    private int currentScore = 0;
    public int CurrentScore {  get { return currentScore;  } set { currentScore = value; ChangeScore(); } }

    private string currentStageName;
    [SerializeField] public string CurrentStageName { get { return currentStageName; } set { currentStageName = value; Debug.Log($"Going To {value} "); } }

    public delegate void ScoreChanged(int score);
    public ScoreChanged onScoreChanged;

    public delegate void OnStageEnded(string stageName, float percentageCompleted, int maxScore);
    public OnStageEnded onStageEnded;

    public void ChangeScore()
    {
        onScoreChanged?.Invoke(currentScore);
    }

    public void EnemyDamaged(float damage)
    {
        CurrentScore += (int)(scoreDatabase.damageToScoreMultiplier * damage);
    }

    public void EnemyKilled(EnemyType enemy)
    {
        if (scoreDatabase.deadEnemyToScorePairs.TryGetValue(enemy, out int value) == false) return;
        CurrentScore += value;
    }

    private void Awake()
    {
        instance = this;

        CurrentStageName = SceneManager.GetActiveScene().name;
    }

    public void ReportScore(int startingEnemies,int currentEnemies)
    {
        onStageEnded?.Invoke(CurrentStageName, 1f-(float)currentEnemies / (float)startingEnemies, CurrentScore);

    }


}
