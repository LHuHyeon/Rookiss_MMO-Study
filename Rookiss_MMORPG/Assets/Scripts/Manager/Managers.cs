using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    // private static = 같은 클래스 안에서만 호출 가능.
    // public static = 다른 클래스에서도 호출 가능.
    private static Managers s_instance; // 유일성이 보장된다.
    private static Managers Instance { get { Init(); return s_instance; } }  // 유일한 매니저를 갖고 온다.

    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();

    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    
    void Start()
    {
        Init();     // 싱글톤
    }

    void Update()
    {
        Input.OnUpdate();   // 키입력 메소드 호출
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
            
            // [ Sound.Init()를 사용안한 이유 ] 
            // "Sound = Instance._sound"인데 Instance안에 Init() 호출 메소드가 있으므로 무한루프에 빠질 수 있다.
            // 그러므로 s_instance를 사용해서 접근한다.
            // (개인적인 생각으로는 s_instance가 생성된 후 호출하는 것이기 때문에 무한루프에 걸리진 않을 것 같다.)
            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        UI.Clear();
        Scene.Clear();
    }
}
