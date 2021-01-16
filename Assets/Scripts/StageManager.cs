using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class StageManager : ScriptableObject
{
    
    public delegate void OnBirdCountChanged(Queue<GameObject> birdQueue);
    public static OnBirdCountChanged onBirdCountChanged;

    public delegate void OnBirdLaunched(GameObject bird);
    public static OnBirdLaunched onBirdLaunched;
    
    [HideInInspector] public Sling sling;
    [HideInInspector] public GameObject[] birdPrefabs;
    private Queue<GameObject> birdQueue = new Queue<GameObject>();

    private Bird currentBird;
    private int startingEnemies;

    public bool DebugMode;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AudioManager audioManager;
    
    [HideInInspector] public EntityController entityController;
    
    private readonly List<Enemy> activeEnemies = new List<Enemy>();

    private const float AllMovingEntitiesStoppedCheckTime = 2f;
    private float allMovingEntitiesStoppedTimer;

    private bool stageRunning; 
    private void BirdCountChanged() => onBirdCountChanged?.Invoke(birdQueue);
    private bool AnyBirdsLeft => birdQueue.Count > 0;
    public void StartUp(Sling sling)
    {
        onBirdLaunched += Launched;
        this.sling = sling;
        Setup();
        SpawnNextBird();
    }

    public void Update()
    {
        if (!stageRunning) return;
        if (!AnyEntitiesMoving())
        {
            allMovingEntitiesStoppedTimer += Time.deltaTime;
            if (allMovingEntitiesStoppedTimer >= AllMovingEntitiesStoppedCheckTime) //game ended, lets check results
            {
                CheckWinConditions();
            }
            return;
        }

        allMovingEntitiesStoppedTimer = 0f;
    }

    private bool AnyEntitiesMoving()
    {
        var movingEntityCount = 0;

        foreach (var movingEntity in entityController.movingEntities)
        {
            if (Math.Abs(movingEntity.velocity.magnitude) > 0.2f || movingEntity.angularVelocity > 0.2f)
            {
                movingEntityCount++;
            }
        }

        return movingEntityCount > 0;
    }
    
    public void CheckWinConditions()
    {
        stageRunning = false;
        Debug.Log("CheckWinConditions" + activeEnemies.Count);

        if (activeEnemies.Count > 0 && AnyBirdsLeft || sling.IsBirdOnSling())
        {
            Continue();
        }
        else if(activeEnemies.Count > 0 && !AnyBirdsLeft && !sling.IsBirdOnSling())
        {
            Lose();
        }
        else
        {
            Win();
        }
    }
    
    private void SpawnNextBird()
    {
        var bird = Instantiate(birdQueue.Dequeue(), sling.transform.position, Quaternion.identity).GetComponent<Bird>();
        bird.Setup(this, audioManager);
        currentBird = bird;
        BirdCountChanged();
    }
    
    private void Setup(){
    
        birdQueue.Clear();
        for (int i = 0; i < birdPrefabs.Length; i++)
        {
            birdQueue.Enqueue(birdPrefabs[i]);
        }
        startingEnemies = 0;
    }

    private void Launched(GameObject obj)
    {
        Debug.Log("Launched");
        stageRunning = true;
        audioManager.PlaySound("Launch");
    }

    private void Win()
    {
        scoreManager.ReportScore(startingEnemies, activeEnemies.Count);
        audioManager.PlaySound("Win");
    }
    
    public void UnregisterMovingEntity([NotNull] Rigidbody2D rb)
    {
        entityController.movingEntities.Remove(rb);
    }
    
    public void RegisterActiveEnemy([NotNull] Enemy enemy)
    {
        activeEnemies.Add(enemy);
        ++startingEnemies;
    }
    public void RegisterMovingEntity([NotNull] Rigidbody2D rb)
    {
        entityController.movingEntities.Add(rb);
    }

    private void Continue()
    {
        if (currentBird != null)
        {
            UnregisterMovingEntity(currentBird.gameObject.GetComponent<Rigidbody2D>());
            Destroy(currentBird.gameObject);
        }
        SpawnNextBird();
    }

    private void Lose()
    {
        onBirdLaunched -= Launched;
        scoreManager.ReportScore(startingEnemies, activeEnemies.Count);
        audioManager.PlaySound("Lose");
    }

    public void UnregisterActiveEnemy([NotNull] Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }
   
}
