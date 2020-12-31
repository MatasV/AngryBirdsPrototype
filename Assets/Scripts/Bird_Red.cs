using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Bird_Red : Bird
{
    private ParticleSystem ps;

    [SerializeField] private float explosionSize = 2f;
    [SerializeField] private float explosionStrength = 2f;

    public sealed override void Setup(StageManager stageManager) 
    {
        base.Setup(stageManager);
        ps = GetComponent<ParticleSystem>();
    }

    protected sealed override void ActivateSpecial()
    {
        if (specialActivated) return;

        var affectedObjects = Physics2D.OverlapCircleAll(transform.position, explosionSize);
        
        foreach (Collider2D col in affectedObjects)
        {
            var rb = col.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            var trajectoryVector = col.transform.position - transform.position;

            if (trajectoryVector.magnitude > 0)
                rb.AddForce(trajectoryVector.normalized * (explosionStrength / trajectoryVector.magnitude), ForceMode2D.Impulse);
        }

        specialActivated = true;

        ps.Play();

        Invoke(nameof(DestroyMyself), ps.main.startLifetime.constantMax);
    }

    private void DestroyMyself()
    {
        stageManager.UnregisterMovingEntity(rigidBody);
        Destroy(gameObject);
    }
}
