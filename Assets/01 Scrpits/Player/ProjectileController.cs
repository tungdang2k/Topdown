using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using static OBjectPoolManager;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject m_hitVFX;
    
    private float m_moveSpeed;
    private float m_lifeTime;
    private int m_damage;
    private string m_shooterTag = "Untagged";

    private TrailRenderer m_trailRenderer;
    private Rigidbody2D m_rigidbody2D;
    private bool m_hasReturned = false;
    private Coroutine lifeRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        m_hasReturned = false;
        if (m_trailRenderer != null)
        {
            // Buộc làm sạch vệt cũ ngay lập tức
            m_trailRenderer.Clear();
            m_trailRenderer.emitting = true;
        }

        lifeRoutine = StartCoroutine(LifeTimer());

        GameManager.Instance.PlayerHealth.OnDeath += OnPlayerDead;
    }

    private void OnDisable()
    {
        if (lifeRoutine != null)
            StopCoroutine(lifeRoutine);

        if (m_rigidbody2D != null)
            m_rigidbody2D.linearVelocity = Vector2.zero;

        GameManager.Instance.PlayerHealth.OnDeath -= OnPlayerDead;
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Return();
    }

    public void SetBulletStats(float speed, float lifeTime, int damage, string shooterTag)
    {
        m_moveSpeed = speed;
        m_lifeTime = lifeTime;
        m_damage = damage;
        m_shooterTag = shooterTag;
    }

    public void Fire(Vector2 dir)
    {
        if (m_rigidbody2D)
        {
            m_rigidbody2D.linearVelocity = dir * m_moveSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_hasReturned) return;
        if (!string.IsNullOrEmpty(m_shooterTag) && other.CompareTag(m_shooterTag))
        {
            return;
        }
       

        ITakeDame takeDame = other.GetComponent<ITakeDame>();

        if (takeDame != null)
        {
            if (m_shooterTag == "Enemy" && !other.CompareTag("Player"))
            {
                OBjectPoolManager.SpawnObject(m_hitVFX, transform.position, Quaternion.identity, poolType: PoolType.particleSystems);
                Return();
                return; 
            }
            takeDame.TakeDame(m_damage, this.transform);
            OBjectPoolManager.SpawnObject(m_hitVFX, transform.position, Quaternion.identity, poolType: PoolType.particleSystems);
            Return();
            return;
        }

        if (other.CompareTag("Collision"))
        {
            OBjectPoolManager.SpawnObject(m_hitVFX, transform.position, Quaternion.identity, poolType: PoolType.particleSystems);
            Return();
           

        }

    }

    private void OnPlayerDead()
    {
        Return();
    }
    private void Return()
    {
        if (m_hasReturned) return;   

        m_hasReturned = true;

        gameObject.SetActive(false);
        OBjectPoolManager.ReturnObjectToPool(gameObject);
    }

}
