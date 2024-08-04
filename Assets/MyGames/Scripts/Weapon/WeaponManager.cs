using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[AddComponentMenu("DangSon/WeaponManager")]
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance
    {
        get => instance;
    }
    private static WeaponManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

    }
    //
    public List<GameObject> weaponSlot = new List<GameObject>();
    public GameObject activeWeaponSlot;
    [Header ("Ammo")]
    public int totalRifeAmmo = 0;
    public int totalPistolAmmo = 0;
    public int MaxTotalAmmo = 200;
    [Header("Grenade")]
    public float throwFore = 40f;
    public GameObject grenadePrefabs;
    public Transform throwableSpwam;
    public float forceMultipler = 0;
    public float forceMultipleLimit = 2f;
    public int granedeCount = 0;
    public Throwable.ThrowableType equippedLethalType;

    // Start is called before the first frame update
    void Start()
    {
        activeWeaponSlot = weaponSlot[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        ActiveWeaponStart();
        GetKey();
    }
    public void SwitchActiveWeaponSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.GetComponentInChildren<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        activeWeaponSlot = weaponSlot[(int)slotNumber];
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.GetComponentInChildren<Weapon>();
            currentWeapon.isActiveWeapon = true;
            
        }
    }
    void GetKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveWeaponSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveWeaponSlot(1);
        }
        if (Input.GetKey(KeyCode.G))
        {
            forceMultipler += Time.deltaTime;
            if (forceMultipler > forceMultipleLimit)
            {
                forceMultipler = forceMultipleLimit;
            }
            if (granedeCount > 0)
            {
                HubManager.Instance.UpdateSlideUI(forceMultipler * 50);
            }
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (granedeCount > 0)
            {
                ThrowGrenade();
            }
            forceMultipler = 0;
            if (granedeCount > 0)
            {
                HubManager.Instance.UpdateSlideUI(forceMultipler * 50);
            }
            else
            {
                HubManager.Instance.UpdateSlideUI(0);
            }
        }
    }
    void ActiveWeaponStart()
    {
        foreach (var item in weaponSlot)
        {
            if (item == activeWeaponSlot)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }

    }

    internal void PickUpWeapon(GameObject objectHitRaycast)
    {
        AddWeaponActiveSlot(objectHitRaycast);
    }

    private void AddWeaponActiveSlot(GameObject objectHitRaycast)
    {
        //nem xuong
        DropCurrenWeapon(objectHitRaycast);
        //nhat len
        PickCurrenWeapon(objectHitRaycast);
    }

    private void PickCurrenWeapon(GameObject pickUpWeapon)
    {
        pickUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);
        Weapon weapon = pickUpWeapon.GetComponent<Weapon>();
        weapon.isActiveWeapon = true;
        weapon.anim.enabled = true;
        pickUpWeapon.transform.localPosition = weapon.localPositionGun;
        //pickUpWeapon.transform.localRotation = weapon.localRotationGun;
    }

    private void DropCurrenWeapon(GameObject pickUpWeapon) //thao tac nem vu khi xuong dat
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent <Weapon>().anim.enabled = false;
            weaponToDrop.transform.SetParent(pickUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickUpWeapon.transform.localPosition; //nem sung theo vi tri
            weaponToDrop.transform.localRotation = pickUpWeapon.transform.localRotation;// nem sung theo chieu xoay
    }
    }

    internal void PickUpAmmo(AmmoBox ammo)
    {
       switch(ammo.amoType)
        {
            case AmmoBox.AmmoType.RifeAmmo:
                totalRifeAmmo += ammo.amountAmo;
                break;
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.amountAmo;
                break;
            default:
                break;
        }
        if (totalPistolAmmo > MaxTotalAmmo)
        {
            totalPistolAmmo = MaxTotalAmmo;
        }
        if (totalRifeAmmo > MaxTotalAmmo)
        {
            totalRifeAmmo = MaxTotalAmmo;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel model)
    {
        switch(model)
        {
            case Weapon.WeaponModel.Pistol:
                return totalPistolAmmo;
            case Weapon.WeaponModel.M16:
                return totalRifeAmmo;
            default:
                return 0;
        }
    }
    internal void DecreseTotalAmmo(int bulletLeft, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M16:
                totalRifeAmmo-= bulletLeft;
                break;
            case Weapon.WeaponModel.Pistol:
                totalPistolAmmo-= bulletLeft;
                break;
        }

    }
    //throw 
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.grenade:
                // PickUpGrenade();
                PickUpThrowableAssetgrenade(Throwable.ThrowableType.grenade);
                break;
        }
    }

    private void PickUpThrowableAssetgrenade(Throwable.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;
            if (granedeCount < 2)
            {
                granedeCount += 1;
                Destroy(InteractiveManager.Instance.hoveredThrowable.gameObject);
                HubManager.Instance.UpdateThrowablesUI();
            }
        }

    }
    private void ThrowGrenade()
    {
        GameObject lethalPrefabs = GetThrowablePrefabs();
        GameObject throwable = Instantiate(lethalPrefabs, throwableSpwam.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwFore * forceMultipler), ForceMode.Impulse);
        throwable.GetComponent<Throwable>().hasBeenThrow = true;
        granedeCount -= 1;
        HubManager.Instance.UpdateThrowablesUI();
    }

    private GameObject GetThrowablePrefabs()
    {
        switch (equippedLethalType)
        {
            case Throwable.ThrowableType.grenade:
                return grenadePrefabs;
        }
        return new();
    }
}
