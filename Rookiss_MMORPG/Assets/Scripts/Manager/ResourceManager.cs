using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티에서 제공하는 명령어를 더 쉽게 사용하기 위한 매니저
public class ResourceManager
{
    // Asset 경로에서 프리팹 읽기
    public T Load<T>(string path) where T : Object
    {
        // 유니티에서 제공하는 Resources 폴더 안에 해당 경로의 프리팹을 읽어오는 클래스
        return Resources.Load<T>(path);
    }

    // 프리팹 생성
    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 프리팹 읽기
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        
        if (prefab == null){
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 해당 프리팹을 parent의 자식 객체로 생성하기
        GameObject go = Object.Instantiate(prefab, parent);

        // 프리팹이 생성될 때 이름 뒤의 (Clone) 없애기
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);

        // 생성
        return go;
    }

    // 오브젝트 삭제
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        
        Object.Destroy(go);
    }
}
