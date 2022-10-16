using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    int score = 0;

    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    // Start를 override 하는 것 보다 Init를 사용하는게 좋음.
    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, ((PointerEventData data) => { go.transform.position = data.position; }), Define.UIEvent.Drag);
        // ↑ 코드로 대체됨.
        // UI_EventHandler evt = go.GetComponent<UI_EventHandler>();
        // evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }

    public void OnButtonClicked(PointerEventData data)
    {
        score++;

        GetText((int)Texts.ScoreText).text = $"점수 : {score}";
    }
}
