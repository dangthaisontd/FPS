using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[AddComponentMenu("DangSon/Weapon")]
public class Weapon : MonoBehaviour
{
    [Header("Gun")]
    public bool isActiveWeapon;
    public bool isShoting, readyToShoot;
    public float shootingdelay = 2f;
    public int bulletsPerBurst = 3; //max
    public int busrtBulletsLeft;
    //
    [Header("BulletDamageEnemy")]
    public int magazineSize = 7;
    public int bulletLeft;
    public float spreadIntensity;
    //
    public GameObject buletPrefabs;
    public Transform bulletSpawm;
    public float bulletVelocity = 100;
    public float bulletPrefabsTime = 3f;
    //
    public bool allowReset;
    [Header("Positon Rotation")]
    public Vector3 localPositionGun;
    public Vector3 localRotationGun;

    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip reloadClip;

    [Header("Sfx MuzlerFlash")]
    public ParticleSystem muzlerFlash;
    [Header("Animation")]
    public Animator anim;
    public float reloadTime = 1f;
    private int isShootingId;
    private int isReloadingId;
    //Reload
    bool isReloading = false;

    public enum ShotingMode
    {
        Single,
        Burst,
        Auto
    }
    public enum WeaponModel
    {
        M16,
        Pistol
    }
    public WeaponModel thisWeaponModel;
    public ShotingMode currentShotingMode;
    private Player player;
    private void Awake()
    {
        readyToShoot = true;
        busrtBulletsLeft = bulletsPerBurst;
        bulletLeft = magazineSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isShootingId = Animator.StringToHash("isShooting");
        isReloadingId = Animator.StringToHash("isReload");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();      
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isActiveWeapon&&!player.isPlayerDead)
        {
            if (currentShotingMode == ShotingMode.Auto)
            {
                isShoting = Input.GetKey(KeyCode.Mouse0);
            }
            else if((currentShotingMode == ShotingMode.Burst)||(currentShotingMode == ShotingMode.Single))
            {
                isShoting = Input.GetKeyDown(KeyCode.Mouse0);
            }
            if(isShoting&&readyToShoot&&bulletLeft>0)
            {    
                FireWeapon();
            }
            if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>0)
            {
                Reload();
            }
        }
        //Debug.Log(transform.localPosition.x + "/" + transform.localPosition.y + "/" + transform.localPosition.z);
    }

    private void Reload()
    {
        //thay dan
        anim.SetTrigger(isReloadingId);
        AudioManager.Instance.PlaySfxPlayer(reloadClip);
        isReloading = true;
        Invoke("ReloadCompleted",reloadTime);
    }
    void ReloadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>magazineSize)
        {
            bulletLeft = magazineSize;
            WeaponManager.Instance.DecreseTotalAmmo(bulletLeft,thisWeaponModel);
        }
        else
        {
            bulletLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreseTotalAmmo(bulletLeft, thisWeaponModel);
        }
        isReloading = false;
    }
    private void FireWeapon()
    {
        readyToShoot = false;
        bulletLeft--;
        AudioManager.Instance.PlaySfxPlayer(shootClip);
        muzlerFlash.Play();
        anim.SetTrigger(isShootingId);
        Vector3 shotingDirection = CaculateDirectionAndSpread().normalized;
        //tao vien dan theo vector o tren
        GameObject bullet = Instantiate(buletPrefabs, bulletSpawm.position, Quaternion.identity);
        bullet.transform.forward = shotingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shotingDirection*bulletVelocity,ForceMode.Impulse);
        //bullet.GetComponent<Rigidbody>().velocity = shotingDirection *bulletVelocity* Time.deltaTime;
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabsTime));
        if (allowReset)
        {
            Invoke("ResetShot", shootingdelay);
            allowReset = false;
        }
        else
        {
            Invoke("FireWeapon", shootingdelay);
        }
    }
    void ResetShot()
    {
        readyToShoot=true;
        allowReset = true;
    }
    IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabsTime)
    {
        yield return new WaitForSeconds(bulletPrefabsTime);
        Destroy(bullet);
    }

    private Vector3 CaculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(200);
        }
        Vector3 direction = targetPoint - bulletSpawm.position;
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        return direction + new Vector3(0, y, z);
    }
}
