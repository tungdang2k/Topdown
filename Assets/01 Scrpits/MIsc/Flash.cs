using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material m_Material;
    [SerializeField] private float m_restoreDefaultMaltime = 0.2f;

    private Material m_defaultMal;
    private SpriteRenderer m_spriteRenderer;
    private EnemyController m_enemyController;

    private void Awake()
    {
        m_enemyController = GetComponentInChildren<EnemyController>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_defaultMal = m_spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {

        m_spriteRenderer.material = m_Material;
        yield return new WaitForSeconds(m_restoreDefaultMaltime);
        m_spriteRenderer.material = m_defaultMal;
       
    }

}
