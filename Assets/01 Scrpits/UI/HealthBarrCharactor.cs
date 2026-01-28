using UnityEngine;
using UnityEngine.UI;

public class HealthBarrCharator : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private HealthControler health;

    private void Awake()
    {
        // Tự lấy Health từ Enemy cha
        health = GetComponentInParent<HealthControler>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnHealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        fillImage.fillAmount = (float)current / max;
    }
}
