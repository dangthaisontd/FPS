using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/BulletDamagePlayer")]
public class BulletDamagePlayer : MonoBehaviour
{
    public float damageBullet = 20f;
    private void OnCollisionEnter(Collision objectHit)
    {
        if (objectHit != null)
        {
            if (objectHit.collider.CompareTag("Player"))
            {
                objectHit.collider.GetComponent<IDamageble>().TakeDamage(damageBullet);
                Destroy(gameObject);
            }
        }
    }
}
