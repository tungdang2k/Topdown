using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static OBjectPoolManager;

public class HealthControler : MonoBehaviour, ITakeDame
{
    [SerializeField] private int m_maxHealth;
    [SerializeField] private float m_knocbackForce = 4f;
    [SerializeField] private GameObject m_damageTextPrefab;
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    public int Current => m_currHealth;
    public int Max => m_maxHealth;

    private Flash m_Flash;
    private KnockBack m_KnockBack;
    private int m_currHealth;
    private bool m_isDead = false;

    private void Awake()
    {
        m_KnockBack = GetComponent<KnockBack>();
        m_Flash = GetComponent<Flash>();
    }
    private void Start()
    {
        m_currHealth = m_maxHealth;
        OnHealthChanged?.Invoke(m_currHealth, m_maxHealth);
    }

    public void TakeDame(int dame, Transform transform)
    {
        if (GameManager.Instance.CurrentState != GameStateType.Gameplay)
            return;
        m_currHealth -= dame;
        OnHealthChanged?.Invoke(m_currHealth, m_maxHealth);
        if (CompareTag("Player"))
        {
            AudioManager.Instance?.PlayPlayerHitSound();
        }
        if (m_damageTextPrefab)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);
            GameObject textObj = OBjectPoolManager.SpawnObject(m_damageTextPrefab, spawnPos, Quaternion.identity);
            if (textObj.TryGetComponent<FloatingText>(out FloatingText ft))
            {
                ft.SetText(dame.ToString());
            }

        }
        StartCoroutine(m_Flash.FlashRoutine());
        if (m_currHealth <= 0)
        {
            if(m_isDead) return;
            m_isDead = true;
            Die();
        }else
        {
          m_KnockBack.GetKnockedBack(transform, m_knocbackForce);
        }
       
    }

    public void ResetHP()
    {
        m_currHealth = m_maxHealth;
        OnHealthChanged?.Invoke(m_currHealth, m_maxHealth);
    }

    public void Heal(int amount)
    {
        m_currHealth += amount;
        if (m_currHealth > m_maxHealth) m_currHealth = m_maxHealth;
        OnHealthChanged?.Invoke(m_currHealth, m_maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
       
    }
}
