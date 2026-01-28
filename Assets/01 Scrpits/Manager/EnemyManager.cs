using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<HealthControler> enemies = new List<HealthControler>();

    public void Register(HealthControler enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void ResetAllEnemies()
    {
        foreach (var e in enemies)
        {
            if (e == null) continue;

            e.gameObject.SetActive(true);
            e.ResetHP();
            e.transform.position = e.GetComponent<EnemySpawnPoint>().SpawnPosition;
        }
    }
}
