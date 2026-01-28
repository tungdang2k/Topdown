using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject closeDoor;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private GameObject m_popupObject;
    [SerializeField] private TextMeshProUGUI m_popupText;
  
    [SerializeField] private string m_sceneToLoad;
    [SerializeField] private string m_textNeedKey = "Cần chìa khóa";
    [SerializeField] private string m_textOpen = "Bấm E để vào";
    [SerializeField] private string m_textUnlock = "Bấm E để mở khóa";
    public string doorID = "Door_Scene1";

    private bool m_isPlayerNear = false;  
    private bool m_isOpened = false;
    void Start()
    {
        if (PlayerPrefs.GetInt(doorID, 0) == 1)
        {
            m_isOpened = true;
            UpdateDoorVisual(true); 
            
        }
        else
        {
            m_isOpened = false;
            UpdateDoorVisual(false);
            
        }

        if (m_popupObject != null) m_popupObject.SetActive(false);
   
    }

    void Update()
    {
        // Nếu Player đang ở gần VÀ có chìa khóa VÀ bấm nút E
        if (m_isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
           
            if (m_isOpened)
            {
                LoadScene();
            }
            else if (GameManager.Instance.KeyManager.HasKey)
            {
                UnlockAndOpen();
                GameManager.Instance.KeyManager.UseKey();
                
            }
            else
            {
                Debug.Log("Chưa có chìa khóa!");
            }
        }
    }

    private void UpdateDoorVisual(bool isOpen)
    {
        closeDoor.SetActive(!isOpen);
        openDoor.SetActive(isOpen);
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            m_isPlayerNear = true;
            // Hiện popup
            if (m_popupObject != null) m_popupObject.SetActive(true);

            if (m_isOpened)
            {
                m_popupText.text = m_textOpen; // Đã mở
            }
            else if (GameManager.Instance.KeyManager.HasKey)
            {
                m_popupText.text = m_textUnlock; // Có chìa
            }
            else
            {
                m_popupText.text = m_textNeedKey; // Không chìa 
            }

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerNear = false;
            if (m_popupObject != null) m_popupObject.SetActive(false);
        }
    }

   
    void UnlockAndOpen()
    {
        m_isOpened = true; 
        PlayerPrefs.SetInt(doorID, 1);
        PlayerPrefs.Save();

        UpdateDoorVisual(true);
        LoadScene();
    }

    void LoadScene()
    {
        if (m_popupObject != null) m_popupObject.SetActive(false);
        SceneManager.LoadSceneAsync(m_sceneToLoad);

        var player = FindFirstObjectByType<PlayerController>();
        var inventory = FindFirstObjectByType<Activeinventory>();

        if (player != null && inventory != null)
        {
            inventory.RegisterPlayer(player);
        }
    }
}
