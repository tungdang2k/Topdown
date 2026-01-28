using System.Collections;
using UnityEngine;
using static OBjectPoolManager;

public class AutoReleaseObject : MonoBehaviour
{

    [SerializeField] private float m_lifeTime = 0.3f;
   
    private void OnEnable()
    {
        StartCoroutine(AutoDestroys());
    }

    IEnumerator AutoDestroys()
    {
        yield return new WaitForSeconds(m_lifeTime);
        OBjectPoolManager.ReturnObjectToPool(gameObject, poolType: PoolType.gameObject);

    }


}
