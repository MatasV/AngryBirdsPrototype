using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [SerializeField] float shakeAmount = 10;
    [SerializeField] float shakeDuration = 0.3f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ShakeScreen()
    {
        StartCoroutine(nameof(Shake));
    }

    IEnumerator Shake()
    {
        while (shakeDuration > 0.01f)
        {
            Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;
            rotationAmount.z = 0;
            shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);
            transform.localRotation = Quaternion.Euler(rotationAmount);
            yield return null;
        }
        transform.localRotation = Quaternion.identity;
    }
}
