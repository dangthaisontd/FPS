using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/Throwable")]
public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;
    bool hasExploded = false;
    public bool hasBeenThrow = false;
    public enum ThrowableType
    {
        None,
        grenade
    }
    [Header("Throwable")]
    public AudioClip throwableClip;
    public ThrowableType throwableType;
    private void Start()
    {
        countdown = delay;
    }
    private void Update()
    {
        if (hasBeenThrow)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0 && !hasExploded)
            {
                Exploded();
                hasExploded = true;
            }
        }
    }

    private void Exploded()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        Instantiate(GameReferences.Instance.granedaExplusionEffect, transform.position, transform.rotation);
        AudioManager.Instance.PlaySfxPlayer(throwableClip);
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var item in colliders)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            if (item.gameObject.GetComponent<Enemy>())
            {
                item.GetComponent<Enemy>().TakeDamage(100);
            }


        }

    }
}
