using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Red : Bird
{
    private bool specialActivated = false;

    [SerializeField] private float explosionSize = 2f;
    [SerializeField] private float explosionStrength = 2f;

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
    }
}
