using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[AddComponentMenu("DangSon/Enemy")]
public class Enemy : MonoBehaviour,IDamageble
{
    [Header("Health Enemy")]
    public float MaxHealth = 100;
    private float currentHealth;
    public bool isEnemyDead = false;
    [Header("Animation Enemy")]
    public int maxAnimation = 4;
    //animation
    private Animator anim;
    private int IsDeadId;
    private int IsDeadDyingId;
    private int IsDeadReactId;
    private int IsDeadLeftId;
    //Stop Enemy AI
    private EnemyAI enemyAI;
    private EnemyShooterAI enemyShooterAI;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        anim = GetComponent<Animator>();
        IsDeadId = Animator.StringToHash("IsDead");
        IsDeadDyingId = Animator.StringToHash("IsDeadDying");
        IsDeadReactId = Animator.StringToHash("IsDeadReact");
        IsDeadLeftId = Animator.StringToHash("IsDeadLeft");
        enemyAI = GetComponent<EnemyAI>();
        enemyShooterAI = GetComponent<EnemyShooterAI>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Dead()
    {
        int randomValue = Random.Range(0,maxAnimation);
        switch(randomValue)
        {
            case 0: anim.SetTrigger(IsDeadId);
                break;
            case 1:
                anim.SetTrigger(IsDeadDyingId);
                break;
            case 2:
                anim.SetTrigger(IsDeadReactId);
                break;
            case 3:
                anim.SetTrigger(IsDeadLeftId);
                break;
        }
        isEnemyDead = true;
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }
        if (enemyShooterAI != null)
        {
            enemyShooterAI.enabled = false;
        }
        agent.isStopped = true;
    }
    public void TakeDamage(float damage)
    {
        if (isEnemyDead == false)
        {
            if (currentHealth > 0)
            {
                currentHealth -= damage;
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
               
                Dead();       
            }
        }
    }
}
