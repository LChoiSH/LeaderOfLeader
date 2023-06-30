using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private List<T> pool;
    private T prefab;

    public ObjectPool(T prefab, int initialSize)
    {
        pool = new List<T>();
        this.prefab = prefab;

        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool(CreateNewObject());
        }
    }

    private T CreateNewObject()
    {
        T newObj = GameObject.Instantiate(prefab);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void AddObjectToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }

    public T GetNextObject(Vector3 instantiatePosition, Quaternion instantiateRotation)
    {
        T returnObj = null;

        foreach (T obj in pool)
        {
            if (!obj.gameObject.activeSelf)
            {
                returnObj = obj;
                break;
            }
        }

        if(returnObj == null)
        {
            returnObj = CreateNewObject();
            AddObjectToPool(returnObj);
        }

        returnObj.transform.position = instantiatePosition;
        returnObj.transform.rotation = instantiateRotation;
        returnObj.gameObject.SetActive(true);

        return returnObj;
    }
}
