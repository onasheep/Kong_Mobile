using System.Collections.Generic;
using UnityEngine;


public static class ResourceManager
{
    public static Dictionary<string, GameObject> objects;

    public static void Init()
    {
        objects = new Dictionary<string, GameObject>();
   

        AddResouces();
    }       // Init()

    // ! ���ҽ� �迭�� �߰�
    public static void AddResouces()
    {
        GameObject[] objResources = Resources.LoadAll<GameObject>(Define.PATH_OBJECTS);

        AddDictionary(objResources, objects);


    }       // AddResource()

    // ! ���ҽ� �迭�� ��ųʸ��� ĳ��
    private static void AddDictionary<T>(T[] resources_, Dictionary<string, T> dictionary_)
    {

        foreach (T resource in resources_)
        {
            Object temp = resource as Object;
            dictionary_.Add(temp.name, resource);
        }
    }       // AddDictionary()
}