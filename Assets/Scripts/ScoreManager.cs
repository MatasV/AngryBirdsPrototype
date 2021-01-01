using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ScoreManager : ScriptableObject
{
    [SerializeField] private LevelDataController levelDataController;
    [SerializeField] private ScoreDatabase scoreDatabase;

    private int currentScore = 0;
    public int CurrentScore {  get { return currentScore;  } set { currentScore = value; ChangeScore(); } }

    private string currentStageName;

    public delegate void ScoreChanged(int score);
    public static ScoreChanged onScoreChanged;

    public delegate void OnStageEnded(string stageName, float percentageCompleted, int maxScore);
    public static OnStageEnded onStageEnded;

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

    public void ReportScore(int startingEnemies,int currentEnemies)
    {
        currentStageName = SceneManager.GetActiveScene().name;
        levelDataController.UpdateLevelData(currentStageName, 1f-(float)currentEnemies / (float)startingEnemies, CurrentScore);
        onStageEnded?.Invoke(currentStageName, 1f-(float)currentEnemies / (float)startingEnemies, CurrentScore);
    }
}