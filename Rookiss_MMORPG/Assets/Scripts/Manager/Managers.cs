using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance; // 유일성이 보장된다.
    private static Managers Instance { get { Init(); return s_instance; } }  // 유일한 매니저를 갖고 온다.

    private InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        Input.OnUpdate();
    }

    // 싱글톤 메소드
    static void Init()
    {
        if (s_instance == null){
            GameObject go = GameObject.Find("@Manager");// 오브젝트 찾기

            if (go == null){
                go = new GameObject{name = "@Manager"}; // 오브젝트 이름 설정
                go.AddComponent<Managers>();            // 컴포넌트 추가
                Debug.Log("@Manager 생성.");
            }

            DontDestroyOnLoad(go);                      // 씬변경될 때 삭제 안됨.
            s_instance = go.GetComponent<Managers>();
        }
    }
}
