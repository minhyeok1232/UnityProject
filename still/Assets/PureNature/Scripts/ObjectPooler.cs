using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // 싱글턴 패턴 사용

    [SerializeField]
    private GameObject pooledObject;
    public int initialPoolSize = 50;

    private List<GameObject> pooledObjects;
    Queue<GameObject> poolingObjectGameObject = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectGameObject.Enqueue(CreateNewObject());
        }
    }
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(pooledObject);
        newObj.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static GameObject GetObject()
    {
        if (Instance.poolingObjectGameObject.Count > 0)
        {
            var obj = Instance.poolingObjectGameObject.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectGameObject.Enqueue(obj);
    }
}