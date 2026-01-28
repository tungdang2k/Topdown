using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private GameManager m_gameManager;
    private void Awake()
    {
        m_gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnResumeButtonClicked()
    {
        m_gameManager.Continue();
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
