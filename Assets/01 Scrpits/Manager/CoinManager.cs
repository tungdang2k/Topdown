using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int Coin { get; private set; }

    public event Action<int> OnCoinChanged;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddCoin(int value)
    {
        Coin += value;
        OnCoinChanged?.Invoke(Coin);
    }
}
