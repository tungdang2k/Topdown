using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public event Action OnDash;
    [Header("Dash Settings")]
    [SerializeField] private float dashBoost = 10f; 
    [SerializeField] private float dashTime = 0.2f; 
    [SerializeField] private float dashCooldown = 3f;

    [Header("Ghost Effect")]
    [SerializeField] private GameObject m_ghostEffect;
    [SerializeField] private float m_ghostDelaySeconds = 0.05f;

    private PlayerController m_playerController;
    private SpriteRenderer m_spriteRenderer;

    private float m_currentCooldown;
    private bool m_isCooldown;
    private bool m_isDashing;
    private bool m_canDash = true; 
    private Coroutine m_dashCoroutine;
    private Image m_dashCooldownImage;

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
    }

    private void Start()
    {
        m_dashCooldownImage = GameManager.Instance.DashCooldownImage;
    }
    private void Update()
    {
        CooldownUpdate();
    }

    public void Dash()
    {
        if (m_canDash && !m_isDashing)
        {
            StartCoroutine(DashProcess());
            AudioManager.Instance?.PlayDashSound();
            OnDash?.Invoke();
        }
    }

    private IEnumerator DashProcess()
    {
        m_isDashing = true;
        m_canDash = false;
        m_isCooldown = true;
        m_currentCooldown = dashCooldown;
        m_dashCooldownImage.fillAmount = 1f;
        // 1. Tăng tốc độ trong PlayerController
        // Lưu ý: PlayerController cần có hàm hoặc biến public để sửa tốc độ
        float originalSpeed = m_playerController.MoveSpeed;
        m_playerController.MoveSpeed += dashBoost;

        // 2. Bắt đầu hiệu ứng Ghost
        if (m_dashCoroutine != null) StopCoroutine(m_dashCoroutine);
        m_dashCoroutine = StartCoroutine(GhostEffectCoroutine());

        // 3. Chờ hết thời gian dash
        yield return new WaitForSeconds(dashTime);

        // 4. Kết thúc dash: Trả lại tốc độ cũ
        m_playerController.MoveSpeed -= dashBoost;
        m_isDashing = false;

        // 5. Tắt hiệu ứng Ghost
        if (m_dashCoroutine != null) StopCoroutine(m_dashCoroutine);

        // (Tùy chọn) Chờ hồi chiêu nếu muốn
         yield return new WaitForSeconds(dashCooldown);
        m_canDash = true;
    }

    private void CooldownUpdate()
    {
        if (!m_isCooldown) return;

        m_currentCooldown -= Time.deltaTime;

         m_dashCooldownImage.fillAmount = m_currentCooldown / dashCooldown;

        if (m_currentCooldown <= 0f)
        {
            m_isCooldown = false;
            m_dashCooldownImage.fillAmount = 0f;
        }
    }


    private IEnumerator GhostEffectCoroutine()
    {
        while (m_isDashing)
        {
            // Đảm bảo bạn có class ObjectPoolManager hoạt động
            GameObject ghost = OBjectPoolManager.SpawnObject(m_ghostEffect, transform.position, Quaternion.identity);

            // Sao chép Sprite hiện tại của nhân vật sang Ghost
            if (m_spriteRenderer != null && ghost.GetComponentInChildren<SpriteRenderer>() != null)
            {
                ghost.GetComponentInChildren<SpriteRenderer>().sprite = m_spriteRenderer.sprite;
                // Nếu ghost bị ngược hướng, chỉnh lại flipX ở đây nếu cần
                ghost.GetComponentInChildren<SpriteRenderer>().flipX = m_spriteRenderer.flipX;
            }

            yield return new WaitForSeconds(m_ghostDelaySeconds);
        }
    }
}