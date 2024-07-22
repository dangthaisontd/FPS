using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/BulletDamageEnemy")]
public class BulletDamageEnemy : MonoBehaviour
{
    public float damageBullet = 20f;
    private void OnCollisionEnter(Collision objectHit)
    {
        if (objectHit != null)
        {
            if(objectHit.collider.CompareTag("Wall"))
            {
                CreateBulletImpactEfect(objectHit);
                Destroy(gameObject);
            }
            if (objectHit.collider.CompareTag("Enemy"))
            {
                BloodBulletImpactEfect(objectHit);
                objectHit.collider.GetComponent<IDamageble>().TakeDamage(damageBullet);
                Destroy(gameObject);
            }
        }
    }
    private void CreateBulletImpactEfect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        GameObject hole = Instantiate(GameReferences.Instance.bulletEffectImpactPrefabs, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectHit.gameObject.transform);
    }
    private void BloodBulletImpactEfect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        GameObject blood = Instantiate(GameReferences.Instance.bloodEffectImpactPrefabs, contact.point, Quaternion.LookRotation(contact.normal));
        blood.transform.SetParent(objectHit.gameObject.transform);
    }
}
