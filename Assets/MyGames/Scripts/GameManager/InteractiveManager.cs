using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/InteractiveManager")]
public class InteractiveManager : MonoBehaviour
{
    public InteractiveManager Instance
    {
        get => instance;
    }
    private static InteractiveManager instance;
    [Header("Weapon")]
    public Weapon hoveredWeapon;
    public AmmoBox hoveredAmmo;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            
            
            //tim sung
            GameObject objectHitRaycast = hit.transform.gameObject;
            if (objectHitRaycast.GetComponent<Weapon>() && objectHitRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
            
                }
                hoveredWeapon = objectHitRaycast.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpWeapon(objectHitRaycast);
                }
            }
            else
            {
                if(hoveredWeapon != null)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            //tim dan
            if (objectHitRaycast.GetComponent<AmmoBox>())
            {
                if (hoveredAmmo)
                {
                    hoveredAmmo.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmo = objectHitRaycast.GetComponent<AmmoBox>();
                hoveredAmmo.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if ((objectHitRaycast.CompareTag("AmmoBoxM16") && WeaponManager.Instance.totalRifeAmmo < 200))
                    {
                        WeaponManager.Instance.PickUpAmmo(hoveredAmmo);
                        Destroy(objectHitRaycast);
                    }
                    if ((objectHitRaycast.CompareTag("AmmoBoxPistol") && WeaponManager.Instance.totalPistolAmmo < 200))
                    {
                        WeaponManager.Instance.PickUpAmmo(hoveredAmmo);
                        Destroy(objectHitRaycast);
                    }

                }
            }
            else
            {
                if (hoveredAmmo != null)
                {
                    hoveredAmmo.GetComponent<Outline>().enabled = false;
                }
            }
        }

    }
    
}
