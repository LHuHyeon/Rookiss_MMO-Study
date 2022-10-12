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
    float _pressedTime;

    public void OnUpdate()
    {
        // UI 클릭 확인
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // 키입력 메소드가 KeyAction안에 존재하는가?
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null){
            if (Input.GetMouseButton(0)){                               // 마우스를 누르고 있을 때
                if (!_pressed){                                         // 꾹 누르지 않은 상태라면
                    MouseAction.Invoke(Define.MouseEvent.PointDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else{
                if (_pressed){
                    if (Time.time < _pressedTime * 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                        
                    MouseAction.Invoke(Define.MouseEvent.PointUp);
                }
                _pressed = false;
                _pressedTime = 0f;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
