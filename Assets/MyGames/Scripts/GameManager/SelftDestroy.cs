using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/SelftDestroy")]
public class SelftDestroy : MonoBehaviour
{
    public float timeForDestroy = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySeft(timeForDestroy));
    }

    IEnumerator DestroySeft(float timeForDestroy)
    {
      yield return new WaitForSeconds(timeForDestroy);
        Destroy(gameObject);
    }

   
}
