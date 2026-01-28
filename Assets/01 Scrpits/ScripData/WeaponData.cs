using UnityEngine;

// Cho phép tạo đối tượng dữ liệu vũ khí trong Unity Editor
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "Default";
    public GameObject weaponPrefab;
    public float fireRate;
    public Sprite weaponIcon;
    public ProjectileController bulletPrefab;
    public string shooterTag;
    public int magazineSize = 30;
    public int maxAmmo = 90;
    public float reloadTime = 1.2f;
    public float bulletSpeed = 15f;      // Tốc độ bay
    public float bulletLifeTime = 1f;    // Thời gian tồn tại (giây)
    public int minDamage;
    public int maxDamage;


    [Header("shogun")]
    public int amountOfBullets = 1; 
    public float spreadAngle = 0f;
}