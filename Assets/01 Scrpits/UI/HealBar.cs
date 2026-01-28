using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealBar : MonoBehaviour
{
    [SerializeField] Image m_fillImage;
    [SerializeField] TMP_Text m_healthText;
    private HealthControler m_playerHealth;
    private void OnEnable()
    {
        TryBindPlayerHealth();
        if (m_playerHealth != null)
            UpdateHealthBar(m_playerHealth.Current, m_playerHealth.Max);
    }

    private void OnDisable()
    {
        Unbind();
    }

    private void TryBindPlayerHealth()
    {
        if (GameManager.Instance == null) return;

        m_playerHealth = GameManager.Instance.PlayerHealth;
        if (m_playerHealth == null) return;

        m_playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void Unbind()
    {
        if (m_playerHealth != null)
        {
            m_playerHealth.OnHealthChanged -= UpdateHealthBar;
            m_playerHealth = null;
        }
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float fillAmount = (float)currentHealth / maxHealth;

        m_fillImage.fillAmount = fillAmount;
        if(m_healthText != null)
            m_healthText.text = $"{currentHealth} / {maxHealth}";
        
    }


}
