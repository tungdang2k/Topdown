using UnityEngine;

public class HomePanel : MonoBehaviour
{
    private GameManager m_gameManager;
    private void Awake()
    {
        m_gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnPlayButtonClicked()
    {
        m_gameManager.Play();
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void OnSettingButtonClicked()
    {
        m_gameManager.Setting();
    }


}
