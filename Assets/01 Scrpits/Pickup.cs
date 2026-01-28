using System.Collections;
using UnityEngine;
using static OBjectPoolManager;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        Health,
        Ammo,
        Coin,
        Key
    }
    [SerializeField] private float m_pickupDistance = 5f;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_accelartionrate;
    [SerializeField] private float m_heightY = 1.5f;
    [SerializeField] private float m_popDuration = 0.8f;
    [SerializeField] private AnimationCurve m_popCurve;
    
    [SerializeField] private PickupType m_pickupType;
    private Vector3 m_moveDir;
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_playerTransform;
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_playerTransform = GameManager.Instance.PlayerTransform;
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }
    private void Update()
    {
        if (m_playerTransform == null)
        {
            return; 
        }
        Vector3 playerPos = m_playerTransform.position;
        float distance = Vector3.Distance(transform.position, playerPos);
        if (distance < m_pickupDistance)
        {
            m_moveDir = (playerPos - transform.position).normalized;
            m_moveSpeed += m_accelartionrate;
        }
        else
        {
            m_moveDir = Vector3.zero;
            m_moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
       m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + (Vector2)(m_moveDir * m_moveSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && gameObject.activeSelf)
        {
            
            DetetchPickupType(collision.gameObject);
            OBjectPoolManager.ReturnObjectToPool(gameObject, poolType: PoolType.gameObject);
        }
    }
    private IEnumerator AnimCurveSpawnRoutine()
    {
       Vector2 startPos = transform.position;
        float randomY  = transform.position.y + Random.Range(-3f, 3f);
        float randomX  = transform.position.x + Random.Range(-2f, 2f);

        Vector2 endPoint  = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < m_popDuration)
        {
            timePassed += Time.deltaTime;
            float linarT = timePassed / m_popDuration;
            float hightT = m_popCurve.Evaluate(linarT);
            float height = Mathf.Lerp(0f, m_heightY, hightT);
            transform.position = Vector2.Lerp(startPos, endPoint, linarT) + new Vector2(0, height);
            yield return null;
        }
    }

    private void DetetchPickupType(GameObject player)
    {
        switch(m_pickupType)
        {
            case PickupType.Health:
                HealthControler playerHealth = player.GetComponent<HealthControler>();
                AudioManager.Instance.PlayHeartSound();
                playerHealth.Heal(10);
                break;
            case PickupType.Ammo:
                var activeWeapon = player.GetComponentInChildren<ActiveWeapon>();
                if (activeWeapon != null && activeWeapon.CurrentActiveWeapon is IAmmoProvider ammo)
                {
                    ammo.AddReserveAmmo(20);
                }
                break;
            case PickupType.Coin:
                CoinManager.Instance.AddCoin(1);
                AudioManager.Instance.PlayCoinSound();
                break;
            case PickupType.Key:
                GameManager.Instance.KeyManager.AddKey();
                break;
        }
    }
}
