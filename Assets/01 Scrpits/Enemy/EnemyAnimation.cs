using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private float m_animationSmoothness = 0.1f;

    private Animator m_anim;
    private Vector2 m_currentMoveDirection = Vector2.zero;
    
    private SpriteRenderer m_spriteRenderer;
    

    private const string INPUT_X_PARAM = "BlendX";
    private const string INPUT_Y_PARAM = "BlendY";
    private const string WALK_PARAM = "walk";
    private const string DEATH = "death";

    void Start()
    {
        m_anim = GetComponentInChildren<Animator>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }


    void Update()
    {
        UpdateAnimation();

       
    }

    public void SetMoveDirection(Vector2 direction)
    {

        m_currentMoveDirection = direction.normalized;

    }
    private void UpdateAnimation()
    {
        if (m_anim == null) return;

        float targetX = m_currentMoveDirection.x; 
        float currentX = m_anim.GetFloat(INPUT_X_PARAM);
        float newX = Mathf.Lerp(currentX, targetX, Time.deltaTime / m_animationSmoothness);


        float targetY = m_currentMoveDirection.y; 
        float currentY = m_anim.GetFloat(INPUT_Y_PARAM);
        float newY = Mathf.Lerp(currentY, targetY, Time.deltaTime / m_animationSmoothness);

        m_anim.SetFloat(INPUT_X_PARAM, newX);
        m_anim.SetFloat(INPUT_Y_PARAM, newY);

        if (m_currentMoveDirection.x > 0.01f) // di chuyển sang phải
        {

            m_spriteRenderer.flipX = false;
        }
        else if (m_currentMoveDirection.x < -0.01f) //  di chuyển sang trái
        {
            // Lật sprite (Flip X = true)
            m_spriteRenderer.flipX = true;
        }

        float speed = m_currentMoveDirection.magnitude;
        m_anim.SetBool(WALK_PARAM, speed > 0.01f);

    }

    public void EnemyDeathAnim()
    {
        m_anim.SetBool(DEATH, true);
    }
}

