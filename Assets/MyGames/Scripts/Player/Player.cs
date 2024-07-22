using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[AddComponentMenu("DangSon/Player")]
public class Player : MonoBehaviour,IDamageble
{
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject bloodUI;
    private Animator anim;
    private int IsPlayerDeadId;
    private PlayerMovement playerMovement;
    private MouseMovement mouseMovement;
    public bool isPlayerDead = false;
    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0)
        {
           
            PlayerDead();
            //currentHealth = 0;
        }
        StartCoroutine(BloodHub());
    }
     IEnumerator BloodHub()
    {
        bloodUI.SetActive(true);
        yield return new WaitForSeconds(1);
        bloodUI.SetActive(false);
    }
    private void PlayerDead()
    {
        //
        Debug.Log("Dead");
        anim.SetTrigger(IsPlayerDeadId);
        playerMovement.enabled = false;
         mouseMovement.enabled = false;
        isPlayerDead = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        IsPlayerDeadId = Animator.StringToHash("IsPlayerDead");
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        mouseMovement = GetComponent<MouseMovement>();
    }
    // Update is called once per frame
}
