using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티에서 제공하는 명령어를 더 쉽게 사용하기 위한 매니저
public class ResourceManager
{
    // Asset 경로에서 프리팹 가져오기
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // 프리팹 생성
    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 프리팹 가져오기
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        
        if (prefab == null){
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 생성
        return Object.Instantiate(prefab, parent);
    }

    // 오브젝트 삭제
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        
        Object.Destroy(go);
    }
}
