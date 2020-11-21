using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Database", menuName = "New Score Database")]
public class ScoreDatabase : ScriptableObject
{
    public int damageToScoreMultiplier = 10;

    public Dictionary<EnemyType, int> deadEnemyToScorePairs = new Dictionary<EnemyType, int>() {
        { EnemyType.PIG_GREEN, 1000 }
    };

    public int extraBirdScore = 2500;
}
