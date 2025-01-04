using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Stack<GameObject> pool;
    private GameObject prefab;
    private Transform parent;
    private int maxObjects;

    public ObjectPool(int initialCount, GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.maxObjects = initialCount;

        pool = new Stack<GameObject>();
        for (int i = 0; i < initialCount; i++)
        {
            AddToPool(CreateObject());
        }
    }

    public void AddToPool(GameObject obj)
    {
        if (pool.Count >= maxObjects)
        {
            Object.Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pool.Push(obj);
    }

    public GameObject GetFromPool()
    {
        // If there is no object available in the pool, create a new one.
        // If it's returned to the pool, it will be added only 
        GameObject newObj;
        if (pool.Count > 0)
        {
            newObj = pool.Pop();
        }
        else
        {
            newObj = CreateObject();
        }

        newObj.SetActive(true);
        return newObj;
    }

    private GameObject CreateObject()
    {
        return Object.Instantiate(prefab, parent);
    }
}
