using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Rigidbody2D),typeof(SpriteRenderer))]
public class Pig : MonoBehaviour, Enemy
{
    private float health = 5f;
    [SerializeField] private EnemyData database;

    [SerializeField] private TMP_Text healthDisplay;

    [SerializeField] private ScoreGainTextController scoreGainTextController;

    private GameObject canvasGameObject;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private StageManager stageManager;
    private float Health
    {
        get => health;
        set
        {
            health = value;
            
            if (health <= 0)
            {
                OnDeath();
            }
            else if(Math.Abs(health - MaxHealth) > 0.5f)
            {
                OnDamaged();
                
            }
        }
    }

    private float MaxHealth;
    private SpriteRenderer rend;
    private bool dead = false;

    private void Start()
    {
        Setup();
        healthDisplay.gameObject.SetActive(stageManager.DebugMode);
    }
    private void OnDestroy()
    {
        stageManager.UnregisterMovingEntity(GetComponent<Rigidbody2D>());
        stageManager.UnregisterActiveEnemy(this);
    }

    public void Setup()
    {
        rend = GetComponent<SpriteRenderer>();
        stageManager.RegisterMovingEntity(GetComponent<Rigidbody2D>());
        stageManager.RegisterActiveEnemy(this);
        MaxHealth = Health;
        scoreGainTextController = GetComponentInChildren<ScoreGainTextController>();

        healthDisplay = transform.Find("DEBUG_Canvas/HealthDisplay").GetComponent<TMP_Text>();

        GameObject canvasGO = new GameObject("CanvasController");
        canvasGO.transform.position = transform.position;
        var FollowScript = canvasGO.AddComponent<FollowXY>();
        FollowScript.Setup(transform);

        healthDisplay.transform.parent.SetParent(canvasGO.transform);
        scoreGainTextController.transform.SetParent(canvasGO.transform);

        canvasGameObject = canvasGO;

        rend.sprite = database.ReturnInitialSprite();
    }
    public void OnDeath()
    {
        Debug.Log("Dead");
        if (!dead)
        {
            dead = true;
            PlayDeathParticles(database.deathParticles);
            Invoke(nameof(DestroyThis), 1f);
            scoreManager.EnemyKilled(EnemyType.PIG_GREEN);
        }
    }

    private void DestroyThis()
    {
        Destroy(canvasGameObject);
        Destroy(this.gameObject);
    }
    private void PlayDeathParticles(GameObject particleSystem)
    {
        var clone = Instantiate(particleSystem.gameObject, transform.position, Quaternion.identity, gameObject.transform);
        clone.GetComponent<ParticleSystem>().Play();
    }
    
    public void OnPlayerLoss()
    {
        rend.sprite = database.ReturnPlayerLossSprite();
    }

    public void OnDamaged()
    {
        ChangeSpriteFromDamage();
        if (stageManager.DebugMode)
        {
            healthDisplay.text = Health.ToString();
        }

    }

    private void ChangeSpriteFromDamage()
    {
        var sprite = database.ReturnSpriteFromHealth(Health, MaxHealth);
        rend.sprite =
            sprite;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var mass = 1f;
        
        if (other.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            mass = other.gameObject.GetComponent<Rigidbody2D>().mass;
        }

        if (other.relativeVelocity.magnitude > 2.0f){

            if (other.gameObject.name.Contains("Bird")) CameraShake.instance?.ShakeScreen();

            var dmg = mass * other.relativeVelocity.magnitude;
            scoreGainTextController.DamageTaken(dmg);
            scoreManager.EnemyDamaged(dmg);
            Health -= dmg;
        }
    }
}
