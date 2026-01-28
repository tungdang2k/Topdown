using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    private GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnHomeButtonClicked()
    {
        m_gameManager.ShowHome();
    }

    
}
