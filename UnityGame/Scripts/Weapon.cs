/*using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;



    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 100f;
    public float bulletPrefabLifeTime = 3f;




    public GameObject muzzelEffect;
    private Animator animator;

    public float reloadTime;
    public int magzineSize, bulletsLeft;
    public bool isReloading;

    public enum WeaponModel
    {
        Pistol1911,
        M4
    }

    public WeaponModel thisWeaponModel;

    public enum Shootingmode
    {
        Single,
        Burst,
        Auto
    }
    public Shootingmode currentshootingMode;


    //Change in script

    private PlayerController playerController;
    private PlayerController.WeaponActions weaponActions;
    private Weapon weapon;

    void Awake()
    {
        playerController = new PlayerController();
        weaponActions = playerController.Weapon;
        weapon = GetComponent<Weapon>();
        weaponActions.Fire.started += ctx => weapon.FireWeapon();
        weaponActions.Reload.started += ctx => weapon.ReloadWeapon();

        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magzineSize;
    }

    // Update is called once per frame
    private void OnEnable()
    {
        weaponActions.Enable();
    }

    private void OnDisable()
    {
        weaponActions.Disable();
    }

    /*private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magzineSize;
    }

    void Update()
    {
        if(bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundM1911.Play();
        }

        
            if(readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

        

        if(AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magzineSize / bulletsPerBurst}";
        }
    }

    public void ReloadWeapon()
    {
        if(bulletsLeft < magzineSize && isReloading == false)
        {
            Reload();
        }
    }

    public void FireWeapon()
    {
        bulletsLeft--;

        muzzelEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        //SoundManager.Instance.shootingSoundM1911.Play();

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;
         
        
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst
        if(currentshootingMode == Shootingmode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }


    private void Reload()
    {
        //SoundManager.Instance.reloadSoundM1911.Play();
        SoundManager.Instance.PlayReloadingSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);

    }

    private void ReloadCompleted()
    {
        bulletsLeft = magzineSize;
        isReloading = false;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);

    }

    void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

   
}
*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 0.1f; // Reduced for rapid fire
    public int weaponDamage;

    // Burst
    public int bulletsPerBurst = 3;
    private int burstBulletsLeft;

    // Spread
    public float spreadIntensity;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 100f;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    private Animator animator;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    private bool isReloading;

    public enum WeaponModel
    {
        Pistol1911,
        M4
    }
    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode;

    private PlayerController playerController;
    private PlayerController.WeaponActions weaponActions;
    private Coroutine rapidFireCoroutine;

    void Awake()
    {
        playerController = new PlayerController();
        weaponActions = playerController.Weapon;
        weaponActions.Fire.started += ctx => StartFiring();
        weaponActions.Fire.canceled += ctx => StopFiring();
        weaponActions.Reload.started += ctx => ReloadWeapon();

        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    private void OnEnable()
    {
        weaponActions.Enable();
    }

    private void OnDisable()
    {
        weaponActions.Disable();
    }

    void Update()
    {
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundM1911.Play();
        }

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
        }
    }

    public void ReloadWeapon()
    {
        if (bulletsLeft < magazineSize && !isReloading)
        {
            Reload();
        }
    }

    private void StartFiring()
    {
        isShooting = true;
        if (currentShootingMode == ShootingMode.Auto)
        {
            rapidFireCoroutine = StartCoroutine(RapidFire());
        }
        else if (readyToShoot && bulletsLeft > 0)
        {
            FireWeapon();
        }
    }

    private void StopFiring()
    {
        isShooting = false;
        if (rapidFireCoroutine != null)
        {
            StopCoroutine(rapidFireCoroutine);
        }
    }

    private IEnumerator RapidFire()
    {
        while (isShooting && bulletsLeft > 0)
        {
            FireWeapon();
            yield return new WaitForSeconds(shootingDelay);
        }
    }

    public void FireWeapon()
    {
        if (!readyToShoot || bulletsLeft <= 0) return;

        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;
        
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        // Burst
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadingSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke(nameof(ReloadCompleted), reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(100);
        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = Random.Range(-spreadIntensity, spreadIntensity);
        float y = Random.Range(-spreadIntensity, spreadIntensity);
        return direction + new Vector3(x, y, 0);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
