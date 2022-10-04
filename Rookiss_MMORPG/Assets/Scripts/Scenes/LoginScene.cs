using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(Managers.Resource.Instantiate("UnityChan"));
        }

        for (int i = 0; i < 7; i++)
        {
            Managers.Resource.Destroy(list[i].gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)){
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear");
    }
}
