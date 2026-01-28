using UnityEngine;
using static OBjectPoolManager;

public class PickUpSpawn : MonoBehaviour
{
    [SerializeField] private GameObject m_goldPrefab;
    [SerializeField] private GameObject m_KeyPrefab;
    [SerializeField] private GameObject[] m_otherItems;

    public void SpawnPickUp()
    {

        if (m_goldPrefab != null)
        {
            int RandomGoldAmount = Random.Range(1, 5);
            for (int i = 0; i < RandomGoldAmount; i++)
            {
                OBjectPoolManager.SpawnObject(m_goldPrefab, transform.position, Quaternion.identity);
            }
        }

        if(m_KeyPrefab != null)
        {
            OBjectPoolManager.SpawnObject(m_KeyPrefab, transform.position, Quaternion.identity);
        }


        if (Random.value <= 0.5f)
        {
            SpawnRandomOtherItem();
        }
    }

    private void SpawnRandomOtherItem()
    {
        if (m_otherItems == null || m_otherItems.Length == 0) return;

        // random item
        int randomIndex = Random.Range(0, m_otherItems.Length);
        GameObject itemToSpawn = m_otherItems[randomIndex];

        if (itemToSpawn != null)
        {
            // random offset 
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            OBjectPoolManager.SpawnObject(itemToSpawn, transform.position + randomOffset, Quaternion.identity);
        }
    }
}
