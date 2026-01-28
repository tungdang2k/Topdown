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

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();

       
    }


    // lấy vị trí enemy 
    public void SetMoveDirection(Vector2 direction)
    {
        // Chuẩn hóa vector hướng để đảm bảo Blend Tree hoạt động chính xác
        m_currentMoveDirection = direction.normalized;

        // Nếu không di chuyển, giữ nguyên hướng nhìn cuối cùng, nhưng đặt vector về (0,0)
        if (m_currentMoveDirection.magnitude < 0.01f)
        {
            // Không cần làm gì nếu chỉ muốn dừng lại nhưng không đổi hướng nhìn
        }
    }
    private void UpdateAnimation()
    {
        if (m_anim == null) return;


        // 1. Lấy giá trị BlendX và BlendY hiện tại từ Animator
        float targetX = m_currentMoveDirection.x; // <-- Đây là giá trị bạn muốn xem
        float currentX = m_anim.GetFloat(INPUT_X_PARAM);
        float newX = Mathf.Lerp(currentX, targetX, Time.deltaTime / m_animationSmoothness);

        // 3. Nội suy (Lerp) giá trị Y về giá trị đích (currentMoveDirection.y)
        float targetY = m_currentMoveDirection.y; // <-- Đây là giá trị bạn muốn xem
        float currentY = m_anim.GetFloat(INPUT_Y_PARAM);
        float newY = Mathf.Lerp(currentY, targetY, Time.deltaTime / m_animationSmoothness);

        m_anim.SetFloat(INPUT_X_PARAM, newX);
        m_anim.SetFloat(INPUT_Y_PARAM, newY);

        if (m_currentMoveDirection.x > 0.01f) // Nếu di chuyển sang phải
        {
            // Không lật (Flip X = false)
            m_spriteRenderer.flipX = false;
        }
        else if (m_currentMoveDirection.x < -0.01f) // Nếu di chuyển sang trái
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

