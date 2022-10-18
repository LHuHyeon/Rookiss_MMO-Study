using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    Coroutine co;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;  // 타입 설정

        Managers.UI.ShowSceneUI<UI_Inven>();    // 인벤토리 UI 생성

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;   // 데이터 가져오기

        gameObject.GetOrAddComponent<CursorController>();   // 마우스 커서 생성

        GameObject _player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(_player);
        
        Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
    }

    public override void Clear()
    {

    }
}
