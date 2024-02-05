using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static partial class GFunc
{
    // ! ���� �ִ� ��Ʈ ������Ʈ�� �̸����� ã�� �����ϴ� �Լ�
    public static GameObject GetRootObj(string objName_)
    {
        GameObject targetObj_ = default;
        Scene activeScene = GetActiveScene();
        GameObject[] rootObjs = activeScene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjs)
        {
            if (rootObject.name == objName_)
            {
                targetObj_ = rootObject;
                return targetObj_;
            }       // if : name_�� ���� �̸��� ������Ʈ�� �����ϸ� ����
            else {  /* Do Nothing */ }
        }       

        Debug.LogWarning("������ �̸��� ��Ʈ ������Ʈ�� �����ϴ�.");
        return targetObj_;
    }       // FindRootObj()

    // ! Ư�� ������Ʈ�� �ڽ� ������Ʈ�� ��� List<> ������ �����ϴ� �Լ�
    public static List<GameObject> GetChildrenObjs(this GameObject targetObj_)
    {
        List<GameObject> objs = new List<GameObject>();
        GameObject childObj = default;

        for (int i = 0; i < targetObj_.transform.childCount; i++)
        {
            childObj = targetObj_.transform.GetChild(i).gameObject;
            objs.Add(childObj);
        }       // loop 
        if (objs.IsValid()) { return objs; }
        else { return default(List<GameObject>); }
    }       // GetChildObjs()

    // ! Ư�� ������Ʈ�� �ڽ� ������Ʈ�� �̸����� ã�� �����ϴ� �Լ�
    public static GameObject GetChildObj(this GameObject targetObj_, string objName_)
    {
        GameObject searchResult = default;
        GameObject searchTarget = default;
        for (int i = 0; i < targetObj_.transform.childCount; i++)
        {
            searchTarget = targetObj_.transform.GetChild(i).gameObject;
            if (searchTarget.name.Equals(objName_))
            {
                searchResult = searchTarget;
                return searchResult;
            }
            else
            {
                searchResult = GetChildObj(searchTarget, objName_);

                if (searchResult == null || searchResult == default) { /* Pass */ }
                else { return searchResult; }
            }
        }       // loop

        return searchResult;
    }       // GetChildObj()


    public static T GetAttachedComponent<T>(this GameObject gameObject_, string componentName) where T : Component
    {
        return gameObject_.GetComponent<T>();
    }


    // ! Ư�� ���۳�Ʈ�� ���̰� ���� ������Ʈ�� �����ϴ� �Լ�
    public static T CreateObj<T>(string objName_) where T : Component
    {
        Debug.Log(typeof(T));
        GameObject createdObj = new GameObject(objName_);
        return createdObj.AddComponent<T>();
    }       // CreateObj()

    // !  Ư�� �̸����� ���� ������Ʈ�� �����ϴ� �Լ�
    public static GameObject CreatObj(this GameObject gameObject_)
    {
        GameObject createdObj = new GameObject(gameObject_.name);
        return createdObj;
    }       // CreateObj()

    public static Scene GetActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        return activeScene;
    }       // GetActiveScene()

}