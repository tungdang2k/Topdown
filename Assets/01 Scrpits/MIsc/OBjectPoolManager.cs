using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
public class OBjectPoolManager : MonoBehaviour
{
   



    [SerializeField] private bool m_addToDontDestroyOnLoad = false;

    private GameObject m_emptyHoder;
    private static GameObject m_particleSystemsEmpty;
    private static GameObject m_gameObjectEmpty;
    private static GameObject m_soundFXEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> m_objectPool;
    private static Dictionary<GameObject, GameObject> m_cloneToPrefabMap;

    public enum PoolType
    {
        particleSystems,
        soundFX, 
        gameObject
    }
    public static PoolType poolingType = PoolType.gameObject;

    private void Awake()
    {
        

        m_objectPool = new Dictionary<GameObject, ObjectPool<GameObject>>();
        m_cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        SetupEmpty();
    }

    private void SetupEmpty()
    {
        m_emptyHoder = new GameObject("Object Pool");

        m_particleSystemsEmpty = new GameObject("Particle Effect");
        m_particleSystemsEmpty.transform.SetParent(m_emptyHoder.transform);

        m_soundFXEmpty = new GameObject("Sound FX");
        m_soundFXEmpty.transform.SetParent(m_emptyHoder.transform);

        m_gameObjectEmpty = new GameObject("GameObject");
        m_gameObjectEmpty.transform.SetParent(m_emptyHoder.transform);

        if (m_addToDontDestroyOnLoad)
        {

            DontDestroyOnLoad(m_particleSystemsEmpty.transform.root);
        }
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.gameObject)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroy

        );

        m_objectPool.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.gameObject)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, pos, rot);
        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);

        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private static void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void OnGetObject(GameObject obj)
    {

    }

    private static void OnDestroy(GameObject obj)
    {
        if (m_cloneToPrefabMap.ContainsKey(obj)) {
            m_cloneToPrefabMap.Remove(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.gameObject:
                return m_gameObjectEmpty;
            case PoolType.particleSystems:
                return m_particleSystemsEmpty;
            case PoolType.soundFX:
                return m_soundFXEmpty;
            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.gameObject) where T : Object
    {
        if (!m_objectPool.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        GameObject obj = m_objectPool[objectToSpawn].Get();

        if (obj != null) {

            if (!m_cloneToPrefabMap.ContainsKey(obj))
            {
                m_cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position  = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject)) {

                return obj as T;
            }
            T component = obj.GetComponent<T>();

            if (component == null) {

                Debug.LogError($"Object {objectToSpawn.name} does't have component of type {typeof(T)}");
                return null;
            }
            return component;
        }

        return null;
    }

    public  static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.gameObject) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.gameObject)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }



    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.gameObject)
    {
        if(m_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {   
            GameObject parentObject = SetParentObject(poolType);
            if(obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }
            if(m_objectPool.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not prefab" + obj.name);
        }

    }
    



}
