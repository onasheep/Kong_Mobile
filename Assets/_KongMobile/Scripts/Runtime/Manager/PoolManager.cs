using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance { get { return instance; } }
    public Dictionary<string, Queue<GameObject>> poolDictinoary;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    void Start()
    {
        poolDictinoary = new Dictionary<string, Queue<GameObject>>();        
    }

    private void SetQueue(string name)
    {
        if(poolDictinoary.ContainsKey(name) == true) { return; }

        Queue<GameObject> tempQue = new Queue<GameObject>();
        poolDictinoary.Add(name, tempQue);
    }

    private void CheckQueue(string name)
    {
        if (poolDictinoary[name].IsValid() == false)
        {
            
             AddToQueue(name);
        }
        else
        {
            /* Do Nothing */ 
        }
    }       // CheckQueue()

    private void AddToQueue(string name)
    {
        GameObject tempObject = Instantiate(ResourceManager.objects[name],this.transform);
        tempObject.name = name;
        poolDictinoary[name].Enqueue(tempObject);
    }

    public GameObject GetFromPool(string name, Vector3 position, Quaternion rotation)
    {
        SetQueue(name);
        CheckQueue(name);

        GameObject getObject = poolDictinoary[name].Dequeue();
        getObject.transform.SetParent(null);
        getObject.transform.position = position;
        getObject.transform.rotation = rotation;
        getObject.SetActive(true);

        return getObject;
    }       // GetFromPool()

    public void ReturnToPool(GameObject returnObject)
    {
        poolDictinoary[returnObject.name].Enqueue(returnObject);
        returnObject.SetActive(false);
        returnObject.transform.SetParent(this.transform);
        returnObject.transform.position = Vector3.zero;
        returnObject.transform.rotation = Quaternion.identity;

    }       // ReturnToPool()


}