using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameStateType
{
    Home,
    Gameplay,
    Pause,
    GameOver,
    GameSetting
}
public class GameManager : Singleton<GameManager>
{
    public Image DashCooldownImage => m_dashCooldownImage;
    public Transform PlayerTransform { get; private set; }
    public HealthControler PlayerHealth { get; private set; }
    public KeyManager KeyManager { get; private set; }
    public bool HasKey { get; private set; }
    public bool IsPaused =>
    m_currentState == GameStateType.Pause;
    public GameStateType CurrentState => m_currentState;

    [SerializeField] private Image m_dashCooldownImage;
    [SerializeField] private GameObject m_homePanel;
    [SerializeField] private GameObject m_pausePanel;
    [SerializeField] private GameObject m_settingPanel;
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private GameObject m_gamePlayPanel;
    [SerializeField] Texture2D m_cursorgun;
    [SerializeField] Texture2D m_cursorDefault;
    [SerializeField] Vector2 m_hotSpot = Vector2.zero;

    private Transform m_playerSpawnPoint;
    private PlayerController m_playerController;
    private HealthControler m_playerHealth;
    private GameStateType m_currentState;

    
    protected override void Awake()
    {
        base.Awake();
        KeyManager = GetComponentInChildren<KeyManager>();
        m_playerController = FindFirstObjectByType<PlayerController>();
        m_playerHealth = m_playerController.GetComponent<HealthControler>();
    }

    private void Start()
    {
        
        m_homePanel.gameObject.SetActive(false);
        m_pausePanel.gameObject.SetActive(false);
        m_settingPanel.gameObject.SetActive(false);
        m_gameOverPanel.gameObject.SetActive(false);
        m_gamePlayPanel.gameObject.SetActive(false);

        SetState(GameStateType.Home);

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        RegisterPlayer(m_playerController.transform);
        RegisterPlayerHealth(m_playerController.GetComponent<HealthControler>());

        m_playerController.transform.position = m_playerSpawnPoint.position;
    }

    public void RegisterPlayer(Transform player)
    {
        PlayerTransform = player;

    }
    public void RegisterPlayerHealth(HealthControler health)
    {
        if (PlayerHealth != null)
            PlayerHealth.OnDeath -= OnPlayerDead;

        PlayerHealth = health;
        PlayerHealth.OnDeath += OnPlayerDead;
    }
    public void AddKey()
    {
        HasKey = true;
    }

    public void UseKey()
    {
        HasKey = false;
    }


    void OnPlayerDead()
    {
        ResetPlayer();
        SetState(GameStateType.GameOver);
        ResetWorld();
    }

    void ResetPlayer()
    {

        m_playerController.transform.position = m_playerSpawnPoint.position;
        m_playerHealth.ResetHP();

    }

    void ResetWorld()
    {
        EnemyManager.Instance.ResetAllEnemies();

    }

    public void SetState(GameStateType state)
    {
        m_currentState = state;
        m_homePanel.gameObject.SetActive(m_currentState == GameStateType.Home);
        m_pausePanel.gameObject.SetActive(m_currentState == GameStateType.Pause);
        m_settingPanel.gameObject.SetActive(m_currentState == GameStateType.GameSetting);
        m_gameOverPanel.gameObject.SetActive(m_currentState == GameStateType.GameOver);
        m_gamePlayPanel.gameObject.SetActive(m_currentState == GameStateType.Gameplay);
        Time.timeScale = IsPaused ? 0f : 1f;

        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (m_currentState == GameStateType.Gameplay)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            if (m_cursorgun != null)
                Cursor.SetCursor(m_cursorgun, m_hotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (m_cursorDefault != null)
                Cursor.SetCursor(m_cursorDefault, Vector2.zero, CursorMode.Auto); 
        }
    }



    public void ShowHome()
    {
        SetState(GameStateType.Home);
    }

    public void Play()
    {
        SetState(GameStateType.Gameplay);
    }

    public void Pause()
    {
        SetState(GameStateType.Pause);
    }

    public void Continue()
    {
        SetState(GameStateType.Gameplay);
    }

    public void Setting()
    {
        SetState(GameStateType.GameSetting);
    }

    public void GameOver()
    {
        SetState(GameStateType.GameOver);
    }


}
