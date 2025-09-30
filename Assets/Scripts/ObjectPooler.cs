using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Dictionary<string, Queue<GameObject>> poolDict;
    public List<Pool> pools;

    // Start is called before the first frame update
    void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.Log("wrong tag");
            return null;
        }
        GameObject objToSpawn = poolDict[tag].Dequeue();

        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;
        objToSpawn.SetActive(true);

        IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();

        // Debug.Log("REUSED!" + pooledObj);

        if (pooledObj != null)
        {
            pooledObj.OnSpawn();
        }

        poolDict[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}
