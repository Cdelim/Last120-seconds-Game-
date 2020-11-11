using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Stack<GameObject> objectPool = new Stack<GameObject>();

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {

            GameObject obje = Object.Instantiate(prefab);
            AddPool(obje);
        }
    }

    public GameObject PopPool()
    {
        if (objectPool.Count > 0)
        {
            GameObject obje = objectPool.Pop();
            obje.gameObject.SetActive(true);
            return obje;
        }
        /*GameObject newObject = Object.Instantiate(prefab);
        newObject.SetActive*/
        return Object.Instantiate(prefab);
    }

    public void AddPool(GameObject obje)
    {
        obje.gameObject.SetActive(false);
        objectPool.Push(obje);
    }
}
