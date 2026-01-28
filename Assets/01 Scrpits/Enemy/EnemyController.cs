using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    Transform Player => GameManager.Instance != null ? GameManager.Instance.PlayerTransform : null;

    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float loseRange = 12f;

    private EnemyAnimation m_EnemyAnimation;
    private KnockBack m_KnockBack;
    private Flash m_Flash;
    private HealthControler m_healthControler;
    private Vector3 m_startingPosition;
    private bool isChasing;
    //private Transform m_playerTransform;
    NavMeshAgent m_NavMeshAgent;

    private void Awake()
    {
        m_KnockBack = GetComponent<KnockBack>();
        m_EnemyAnimation = GetComponent<EnemyAnimation>();
        m_Flash = GetComponent<Flash>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_healthControler = GetComponent<HealthControler>();
        //m_NavMeshAgent.updatePosition = false;
        m_NavMeshAgent.updateUpAxis = false;
        m_NavMeshAgent.updateRotation = false;

    }
    void Start()
    {
        //if (GameManager.Instance != null)
        //    m_playerTransform = GameManager.Instance.PlayerTransform;
        m_startingPosition = transform.position;

    }

    private void OnEnable()
    {
        m_startingPosition = transform.position;
        isChasing = false;
        m_healthControler.OnDeath += DetectDeath;
    }

    private void OnDisable()
    {
        m_healthControler.OnDeath -= DetectDeath;
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentState != GameStateType.Gameplay)
        {
            m_NavMeshAgent.isStopped = true;
            return;
        }
        if (m_KnockBack.gettingKnockBack)
        {
            // Vô hiệu hóa Agent khi bị đẩy lùi
            m_NavMeshAgent.isStopped = true;
            return;
        }

        // Kích hoạt Agent trở lại
        if (m_NavMeshAgent.isStopped)
        {
            m_NavMeshAgent.isStopped = false;
        }


        EnemyChase();
    }

    private void EnemyChase()
    {
        if(Player == null) return;
        float distance = Vector3.Distance(transform.position, Player.position);
               
        if (TransparentDetection.tranparent)
        {
            isChasing = false;
            ReturnToStart();
            return; 
        }
        if (isChasing)
        {
            if (distance > loseRange)
            {
                isChasing = false;
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            
            if (distance < detectRange)
            {
                isChasing = true; // Bắt đầu đuổi
            }
            else
            {
                ReturnToStart();
            }
        }
    }

    private void ChasePlayer()
    {
        m_NavMeshAgent.speed = m_moveSpeed;

        m_NavMeshAgent.SetDestination(Player.position);
        Vector2 directionPlayer = m_NavMeshAgent.velocity;


        if (directionPlayer.sqrMagnitude > 0.1f) // Kiểm tra nếu đang di chuyển
        {
            m_EnemyAnimation.SetMoveDirection(directionPlayer);
        }
    }

   private void ReturnToStart()
    {
        
        m_NavMeshAgent.speed = m_moveSpeed;
        m_NavMeshAgent.SetDestination(m_startingPosition);
        Vector2 dir = m_NavMeshAgent.velocity;
        m_EnemyAnimation.SetMoveDirection(dir);
    }

    public void DetectDeath() {
        if (gameObject != null)
        {
            m_KnockBack.GetKnockedBack(Player, 0f);
            GetComponent<PickUpSpawn>().SpawnPickUp();
            m_EnemyAnimation.EnemyDeathAnim();
            AudioManager.Instance.PlayEnemyDieSound();
            Destroy(gameObject, 0.2f);
        }
 
    }
    
}