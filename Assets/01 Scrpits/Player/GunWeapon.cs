using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunWeapon : MonoBehaviour, IWeapon, IAmmoProvider
{
    [SerializeField] private GameObject m_projectilePrefab;

    [SerializeField] private Transform m_aimTransform;
    [SerializeField] private Transform m_firePos;
    [SerializeField] private WeaponData m_weaponData;
    public float FireRate => m_weaponData.fireRate;
    private float m_bulletSpeed;
    private float m_bulletLifeTime;

    private int runtimeCurrentAmmo;
    private int runtimeReserveAmmo;

    //public int CurrentAmmo => runtimeCurrentAmmo;
    public int CurrentAmmo
    {
        get { return runtimeCurrentAmmo; }
        private set { runtimeCurrentAmmo = value; }
    }

    public int ReserveAmmo
    {
        get { return runtimeReserveAmmo; }
        private set { runtimeReserveAmmo = value; }
    }
    public int MagazineSize => m_weaponData.magazineSize;

    public bool IsReloading { get; private set; }
    private int m_minDamage;
    private int m_maxtDamage;
    private int m_bulletsPerShot;
    private float m_spreadAngle;
    private float m_cooldownTime;
    private string m_shooterTag;
    private float m_nextAttackTime = 0f;
    private float m_reloadTime;
    private ProjectileController m_currentBulletPrefab;
    private SpriteRenderer m_spriteRenderer;
    private bool m_initialized = false;



    private void Awake()
    {
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
       
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        IsReloading = false;
    }

    void Update()
    {
      
        RotationGun();
    }


    public void InitializeGun(WeaponData data)
    {
        if (m_initialized) return;
        m_initialized = true;
        m_nextAttackTime = 0f;
        if (m_spriteRenderer != null && data.weaponIcon != null)
        {
            m_spriteRenderer.sprite = data.weaponIcon;
        }
        m_currentBulletPrefab = data.bulletPrefab;
        m_bulletSpeed = data.bulletSpeed;
        m_bulletLifeTime = data.bulletLifeTime;
        m_minDamage = data.minDamage;
        m_maxtDamage = data.maxDamage;
        m_bulletsPerShot = data.amountOfBullets;
        m_spreadAngle = data.spreadAngle;
        m_cooldownTime = data.fireRate;
        m_shooterTag = data.shooterTag;
        m_reloadTime = data.reloadTime;

        runtimeCurrentAmmo = m_weaponData.magazineSize;
        runtimeReserveAmmo = m_weaponData.maxAmmo;
    }

    private void RotationGun()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPosition - m_aimTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        m_aimTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (angle > 90 || angle < -90)
        {
            m_spriteRenderer.flipY = true;
        }
        else
        {
            m_spriteRenderer.flipY = false;
        }
    }


    public void Attack()
    {
        if(runtimeCurrentAmmo <= 0)
        {
            Reload();
            return;
        }
        if (m_currentBulletPrefab == null || m_firePos == null || Time.time < m_nextAttackTime) return;


        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        Vector2 baseShootDir = (mouseWorldPosition - m_firePos.position).normalized;


        float baseAngle = Mathf.Atan2(baseShootDir.y, baseShootDir.x) * Mathf.Rad2Deg;
        
        for (int i = 0; i < m_bulletsPerShot; i++)
        {
            

            float randomSpread = Random.Range(-m_spreadAngle / 2f, m_spreadAngle / 2f);

            float finalAngle = baseAngle + randomSpread;
            Vector3 spawnPos = m_firePos.position + (Vector3)(baseShootDir * 0.15f);
            spawnPos += (Vector3)(Vector2.Perpendicular(baseShootDir) * (0.05f * (i - m_bulletsPerShot / 2f)));

            Quaternion finalRotation = Quaternion.Euler(0, 0, finalAngle);
            ProjectileController projectile = OBjectPoolManager.SpawnObject(m_currentBulletPrefab, spawnPos, finalRotation);

            if (projectile != null)
            {
                int m_bulletDamage = Random.Range(m_minDamage, m_maxtDamage + 1);
                projectile.SetBulletStats(m_bulletSpeed, m_bulletLifeTime, m_bulletDamage, m_shooterTag);
                    
                Vector2 spreadDir = new Vector2(
                    Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                    Mathf.Sin(finalAngle * Mathf.Deg2Rad)
                    );

                projectile.Fire(spreadDir);
            }
           
            if(m_bulletsPerShot <= 1)
            {
                AudioManager.Instance.PlayRifleSound();
            }
            else
            {
                AudioManager.Instance.PlayShotgunSound();
            }
            
        }
        runtimeCurrentAmmo--;
        m_nextAttackTime = Time.time + m_cooldownTime;

    }

    public void AddReserveAmmo(int amount)
    {
        runtimeReserveAmmo = Mathf.Clamp(
            runtimeReserveAmmo + amount,
            0,
            m_weaponData.maxAmmo
        );
    }
    public void Reload()
    {
        if (IsReloading) return;
        if (CurrentAmmo == MagazineSize) return;
        if (ReserveAmmo <= 0) return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        yield return new WaitForSeconds(m_reloadTime);

        int need = MagazineSize - CurrentAmmo;
        int take = Mathf.Min(need, ReserveAmmo);

        runtimeCurrentAmmo += take;
        ReserveAmmo -= take;

        IsReloading = false;
    }

    public WeaponData GetWeaponData()
    {
        return m_weaponData;
    }
}
