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

    public delegate void OnBirdCountChanged(int BirdsLeft);

    public OnBirdCountChanged onBirdCountChanged;


    public List<Rigidbody2D> movingEntities = new List<Rigidbody2D>();
    private List<Enemy> activeEnemies = new List<Enemy>();
    private Sling sling;
    
    [SerializeField] private GameObject birdPrefab;
    private Queue<GameObject> birdQueue = new Queue<GameObject>();

    private Bird currentBird = null;

    public bool DebugMode = false;

    private void BirdCountChanged()
    {
        onBirdCountChanged.Invoke(birdQueue.Count);
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
    { //Invoke delegate
        var birdGO = Instantiate(birdQueue.Dequeue(), sling.transform.position, Quaternion.identity);
        var bird = birdGO.GetComponent<Bird>();
        bird.Setup();
        currentBird = bird;
        BirdCountChanged();
    }
    private void Setup()
    {
        for (int i = 0; i < 3; i++)
        {
            birdQueue.Enqueue(birdPrefab);
        }
    }
    public void Launched()
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
    }
    
    private static void Restart()
    {
        Debug.Log("Time To Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterMovingEntity([NotNull]Rigidbody2D rb)
    {
        movingEntities.Add(rb);
    }

    private void Win()
    {
        Debug.Log("Win!");
    }

    private void Continue()
    {
        Debug.Log("Next Life!");
        UnregisterMovingEntity(currentBird.gameObject.GetComponent<Rigidbody2D>());
        Destroy(currentBird.gameObject);
        sling.gameObject.SetActive(true);
        SpawnNextBird();
    }
    private void Lose()
    {
        Debug.Log("Lose");
    }
    public void UnregisterMovingEntity([NotNull] Rigidbody2D rb)
    {
        movingEntities.Remove(rb);
    }
    
    public void RegisterActiveEnemy([NotNull] Enemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    private void CheckWinConditions()
    {
        if (activeEnemies.Count > 0 && birdQueue.Count > 0)
        {
            Continue();
        }
        else if(activeEnemies.Count > 0 && birdQueue.Count == 0)
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
