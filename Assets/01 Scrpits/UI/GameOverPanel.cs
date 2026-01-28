using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private GameManager m_gameManager;
    [SerializeField] private TMP_Text m_coinText;
    private void Awake()
    {
        m_gameManager = FindFirstObjectByType<GameManager>();
       
    }

    private void Start()
    {
        UpdateUI(CoinManager.Instance.Coin);
    }

    private void UpdateUI(int coin)
    {
        if (m_coinText != null)
            m_coinText.text = "Gold " +  coin.ToString("D3");
    }
    public void OnHomeButtonClicked()
    {
        m_gameManager.ShowHome();
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

}
