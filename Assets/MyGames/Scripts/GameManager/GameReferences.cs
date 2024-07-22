using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/GameReferences")]
public class GameReferences : MonoBehaviour
{
   
    public static GameReferences Instance
    {
        get => instance;
    }
    private static GameReferences instance;

    public GameObject bulletEffectImpactPrefabs;
    public GameObject bloodEffectImpactPrefabs;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }
}
