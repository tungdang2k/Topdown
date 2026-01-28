using UnityEngine;
using static OBjectPoolManager;

public class Destructible : MonoBehaviour, ITakeDame
{
    [SerializeField] private int m_maxHp = 5;
    [SerializeField] private GameObject m_DestroyVFX;
    [SerializeField] private GameObject m_hitVFX;
    private bool m_isDestroy = false;

    private int m_currentHp;

    private void Start()
    {
        m_currentHp = m_maxHp;


    }
    public void TakeDame(int dame, Transform transform)
    {
        m_currentHp -= dame;
         
        DetectDeath();
    }

    public void DetectDeath()
    {
        if(m_currentHp < 0)
        {
           if (m_isDestroy) return;
            m_isDestroy = true;
            GameObject ghost = OBjectPoolManager.SpawnObject(m_DestroyVFX, transform.position, Quaternion.identity, poolType: PoolType.particleSystems);
            GetComponent<PickUpSpawn>().SpawnPickUp();
            AudioManager.Instance?.PlayCrateDestructSound();
            Destroy(gameObject);
        }

    }

}
