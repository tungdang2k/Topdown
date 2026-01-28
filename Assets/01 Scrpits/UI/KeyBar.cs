using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyBar : MonoBehaviour
{
     public void Refresh(bool hasKey)
    {
        gameObject.SetActive(hasKey);
    }
}

