using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<GameObject> DamageHands;
    private Enemy enemy;
    bool isEnable = false;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isEnable == true) return;
        if (enemy != null && DamageHands.Count > 0&&enemy.isEnemyDead)
        {
            foreach (var item in DamageHands)
            {
                item.SetActive(false);
                isEnable = true;
            }
        }
    }
}
