using System;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private KeyBar keyBar;
    public static KeyManager Instance { get; private set; }
    public bool HasKey { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddKey()
    {
        HasKey = true;
        keyBar.Refresh(true);
    }

    public void UseKey()
    {
        HasKey = false;
        keyBar.Refresh(false);
    }

}
