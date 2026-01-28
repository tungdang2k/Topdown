using TMPro;
using UnityEngine;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private TMP_Text m_coinText;

    private void Start()
    {
        CoinManager.Instance.OnCoinChanged += UpdateUI;
        UpdateUI(CoinManager.Instance.Coin);
    }

    private void OnDestroy()
    {
        if (CoinManager.Instance != null)
            CoinManager.Instance.OnCoinChanged -= UpdateUI;
    }

    private void UpdateUI(int coin)
    {
        if (m_coinText != null)
            m_coinText.text = coin.ToString("D3");
    }
}
