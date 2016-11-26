using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolingSystem : MonoBehaviour {

    List<GameObject> poolObjects;
    public GameObject type;
    public int poolCount;
    public int amountOfItems = 50;


	// Use this for initialization
	void Start () {
        createPool(type, amountOfItems);
        poolCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createPool(GameObject type, int amount)
    {
        poolObjects = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject info = Instantiate(type, Vector3.zero, Quaternion.identity) as GameObject;
            info.GetComponent<EnemyHealth>().pool = gameObject;
            info.SetActive(false);
            poolObjects.Add((info));
            DontDestroyOnLoad(info);
        }
    }

    public GameObject GetObject()
    {
        for(int i = 0; i < poolObjects.Count; i++) {
            if (poolObjects[i].activeSelf == false)
            {
                poolObjects[i].SetActive(true);
                poolCount++;
                return poolObjects[i];
            }
            
        }
        GameObject item = Instantiate(type, Vector3.zero, Quaternion.identity) as GameObject;
        item.transform.SetParent(transform, false);
        return item;
    }

    public void returnToPool(GameObject obj)
    {
        if (poolObjects.Contains(obj) && obj.activeSelf)
        {
            obj.SetActive(false);
            poolCount--;
            
        }
        else
        {
            Destroy(obj);
        }
    }

    public void ClearPool()
    {
        poolObjects.Clear();

        poolObjects = null;
    }


}
