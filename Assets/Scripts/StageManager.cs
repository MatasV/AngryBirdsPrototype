using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public delegate void OnBirdCountChanged(Queue<GameObject> birdQueue);
    public OnBirdCountChanged onBirdCountChanged;

    public delegate void OnBirdLaunched(GameObject bird);
    public OnBirdLaunched onBirdLaunched;
    
    public List<Rigidbody2D> movingEntities = new List<Rigidbody2D>();
    private List<Enemy> activeEnemies = new List<Enemy>();
    private Sling sling;
    
    [SerializeField] private GameObject[] birdPrefabs;
    private Queue<GameObject> birdQueue = new Queue<GameObject>();

    private Bird currentBird = null;
    private int startingEnemies = 0;

    public bool DebugMode = false;
    
    
    private void BirdCountChanged()
    {
        onBirdCountChanged?.Invoke(birdQueue);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
    private void Start()
    {
        Setup();
        SpawnNextBird();
    }

    private void SpawnNextBird()
    {
        var birdGO = Instantiate(birdQueue.Dequeue(), sling.transform.position, Quaternion.identity);
        var bird = birdGO.GetComponent<Bird>();
        bird.Setup();
        currentBird = bird;
        BirdCountChanged();
    }
    private void Setup()
    {
        for (int i = 0; i < birdPrefabs.Length; i++)
        {
            birdQueue.Enqueue(birdPrefabs[i]);
        }
    }
    public void Launched(GameObject obj)
    {
        StartCoroutine(CheckVelocityForRoundEnd());
    }
    private void Awake()
    {
        sling = FindObjectOfType<Sling>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        onBirdLaunched += Launched;
    }
    
    private static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterMovingEntity([NotNull]Rigidbody2D rb)
    {
        movingEntities.Add(rb);
    }

    private void Win()
    {
        ScoreManager.instance.ReportScore(startingEnemies, activeEnemies.Count);
    }

    private void Continue()
    {
        if (currentBird != null)
        {
            UnregisterMovingEntity(currentBird.gameObject.GetComponent<Rigidbody2D>());
            Destroy(currentBird.gameObject);
        }
        sling.gameObject.SetActive(true);
        SpawnNextBird();
    }

    private void Lose()
    {
        ScoreManager.instance.ReportScore(startingEnemies, activeEnemies.Count);
    }

    public void UnregisterMovingEntity([NotNull] Rigidbody2D rb)
    {
        movingEntities.Remove(rb);
    }
    
    public void RegisterActiveEnemy([NotNull] Enemy enemy)
    {
        activeEnemies.Add(enemy);
        startingEnemies++;
    }

    public void CheckWinConditions()
    {
        if (activeEnemies.Count > 0 && birdQueue.Count > 0 || sling.IsBirdOnSling())
        {
            Continue();
        }
        else if(activeEnemies.Count > 0 && birdQueue.Count == 0 && !sling.IsBirdOnSling())
        {
            Lose();
        }
        else
        {
            Win();
        }
    }
    
    public void UnregisterActiveEnemy([NotNull] Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }
    private IEnumerator CheckVelocityForRoundEnd()
    {
        var movingEntityCount = 0;

        foreach (var movingEntity in movingEntities)
        {
            if (Math.Abs(movingEntity.velocity.magnitude) > 0.2f || movingEntity.angularVelocity > 0.2f)
            {
                movingEntityCount++;
            }
        }

        if (movingEntityCount < 1)
        {
            yield return new WaitForSeconds(2f);
            
            movingEntityCount = 0;

            foreach (var movingEntity in movingEntities)
            {
                if (movingEntity.velocity.magnitude > 0.2f || movingEntity.angularVelocity > 0.2f)
                {
                    movingEntityCount++;
                }
            }

            if (movingEntityCount < 1) //game ended, lets check results
            {
                CheckWinConditions();
                StopAllCoroutines();
            }
        }
        
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(CheckVelocityForRoundEnd());
    }
}
