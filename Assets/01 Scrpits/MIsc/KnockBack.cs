using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool gettingKnockBack { get; private set; }
    [SerializeField] private float m_knockTime = 0.1f;
    
    private Rigidbody2D m_Rigidbody2;

    private void Awake()
    {
        m_Rigidbody2 = GetComponent<Rigidbody2D>();

    }

    public void GetKnockedBack(Transform dameSource, float knockBackThurt)
    {
        if (this == null || gameObject == null || dameSource == null) return;
        gettingKnockBack = true;
        Vector2 diference = (transform.position - dameSource.position).normalized * knockBackThurt * m_Rigidbody2.mass;
        m_Rigidbody2.AddForce(diference, ForceMode2D.Impulse);
        StartCoroutine(KnockTime());
    }

    private IEnumerator KnockTime()
    {
        yield return new WaitForSeconds(m_knockTime);
        m_Rigidbody2.linearVelocity = Vector2.zero;
        gettingKnockBack = false;
    }

}
    