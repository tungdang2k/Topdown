using System.Collections;
using UnityEngine;

public enum AttackPattern
{
    Shotgun,
    MachineGun,
    Spiral
}
public class EnemyShooter : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private EnemyData m_data;

    private float m_lastAttackTime = 0f;
    private bool m_isAttacking = false;
    private float m_currentSpiralAngle = 0f;
    private Transform m_playerTransform;

    private void Start()
    {
        m_playerTransform = GameManager.Instance.PlayerTransform;
    }

    private void Update()
    {
        
        if (m_playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, m_playerTransform.position);

        if (distance <= m_data.attackRange)
        {
            // Dùng m_data.attackCooldown
            if (Time.time >= m_lastAttackTime + m_data.attackCooldown && !m_isAttacking)
            {
                m_lastAttackTime = Time.time;

                Attack();
            }
        }
    }

    public void Attack()
    {
       
        if (!m_isAttacking && m_data.availablePatterns.Length > 0)
        {
            StartCoroutine(ExecuteRandomPattern());
        }
    }

    private IEnumerator ExecuteRandomPattern()
    {
        m_isAttacking = true;

        // Random dựa trên danh sách trong Data
        int randomIndex = Random.Range(0, m_data.availablePatterns.Length);
        AttackPattern selectedPattern = m_data.availablePatterns[randomIndex];

        switch (selectedPattern)
        {
            case AttackPattern.Shotgun:
                yield return StartCoroutine(FireShotgun());
                break;

            case AttackPattern.MachineGun:
                yield return StartCoroutine(FireMachineGun());
                break;

            case AttackPattern.Spiral:
                yield return StartCoroutine(FireSpiral());
                break;
        }

        m_isAttacking = false;
    }

    private IEnumerator FireShotgun()
    {
        Vector2 mainDirection = (m_playerTransform.position - transform.position).normalized;

        
        float startAngle = -m_data.spreadAngle / 2f;
        float angleStep = m_data.bulletCount > 1 ? m_data.spreadAngle / (m_data.bulletCount - 1) : 0;

        for (int i = 0; i < m_data.bulletCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 bulletDir = RotateVector(mainDirection, currentAngle);
            SpawnBullet(bulletDir);
        }
        yield return null;
    }

    private IEnumerator FireMachineGun()
    {
        for (int i = 0; i < m_data.bulletCount; i++)
        {
            Vector2 direction = (m_playerTransform.position - transform.position).normalized;
            SpawnBullet(direction);
           
            yield return new WaitForSeconds(m_data.timeBetweenBursts);
        }
    }

    private IEnumerator FireSpiral()
    {
        int bossSpiralCount = 20;
        for (int i = 0; i < bossSpiralCount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(m_currentSpiralAngle * Mathf.Deg2Rad), Mathf.Sin(m_currentSpiralAngle * Mathf.Deg2Rad));
            SpawnBullet(dir);
            // Lấy rotationSpeed từ Data
            m_currentSpiralAngle += m_data.rotationSpeed;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnBullet(Vector2 direction)
    {
        GameObject projectile = OBjectPoolManager.SpawnObject(m_data.projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.right = direction;

        ProjectileController projectileScript = projectile.GetComponent<ProjectileController>();
        if (projectileScript != null)
        {
            projectileScript.SetBulletStats(m_data.bulletSpeed, 3f, m_data.bulletDamage, gameObject.tag);
            projectileScript.Fire(direction);
        }
    }

    private Vector2 RotateVector(Vector2 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
}