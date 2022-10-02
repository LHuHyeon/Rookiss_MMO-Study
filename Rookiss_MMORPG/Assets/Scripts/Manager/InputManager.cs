using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InputManager
{
    // 키 입력 메소드들을 한번에 실행하기 위한 변수
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;  // 마우스 꾹 눌리는 여부

    public void OnUpdate()
    {
        // UI 클릭 확인
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // 키입력 메소드가 KeyAction안에 존재하는가?
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null){
            if (Input.GetMouseButton(0)){
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else{
                if (_pressed)
                    MouseAction.Invoke(Define.MouseEvent.Click);
                _pressed = false;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
