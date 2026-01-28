using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Default";
    public int health = 10;
    public float moveSpeed = 2f;
    public int damage = 1;
    
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;


    public AttackPattern[] availablePatterns;

    [Header("Bullet Settings")]
    public float bulletSpeed = 10f;
    public int bulletDamage = 10;
    public int bulletCount = 3;
    public float spreadAngle = 30f;      // (Shotgun)
    public float timeBetweenBursts = 0.2f; // (MachineGun)
    public float rotationSpeed = 10f;

}
