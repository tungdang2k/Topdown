using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float m_transparentAmout = 0.7f;
    [SerializeField] private float m_fadeTime = 0.4f;
    public static bool tranparent { get; private set; }


    private SpriteRenderer m_spriteRenderer;
    private Tilemap m_tilemap;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_tilemap = GetComponent<Tilemap>();
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;
        if (collision.gameObject.CompareTag("Player"))
        {   
            tranparent = true;
            if (m_spriteRenderer != null)
            {
                StartCoroutine(FadeRoutine(m_spriteRenderer, m_fadeTime, m_spriteRenderer.color.a, m_transparentAmout));
            }
            else
            {
                StartCoroutine(FadeRoutine(m_tilemap, m_fadeTime, m_tilemap.color.a, m_transparentAmout));
            }

        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            tranparent = false;
            if (m_spriteRenderer != null)
            {
                StartCoroutine(FadeRoutine(m_spriteRenderer, m_fadeTime, m_spriteRenderer.color.a, 1f));
            }
            else
            {
                StartCoroutine(FadeRoutine(m_tilemap, m_fadeTime, m_tilemap.color.a, 1f));

            }

        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer sp, float fadeTime, float startValue, float tagetTransparency)
    {
        float elapedTime = 0f;

        while (elapedTime < fadeTime) { 
            
            elapedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, tagetTransparency, elapedTime/fadeTime);
            m_spriteRenderer.color = new Color(sp.color.r, sp.color.g, sp.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float tagetTransparency)
    {
        float elapedTime = 0f;

        while (elapedTime < fadeTime)
        {
            elapedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, tagetTransparency, elapedTime / fadeTime);
            m_tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }

}
