using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Vector3 SpawnPosition { get; private set; }

    private void Awake()
    {
        SpawnPosition = transform.position;

    }

    private void Start()
    {
        EnemyManager.Instance.Register(GetComponent<HealthControler>());
    }
}
