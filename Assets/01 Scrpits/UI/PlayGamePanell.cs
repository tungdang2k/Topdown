using UnityEngine;

public class PlayGamePanel : MonoBehaviour
{
   private GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnPauseButtonClicked()
    {
        m_gameManager.Pause();
    }
}
