using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager
{
    // 키 입력 메소드들을 한번에 실행하기 위한 변수
    public Action KeyAction = null;

    public void OnUpdate()
    {
        // 키 입력이 있는가?
        if (Input.anyKey == false)
            return;

        // 키입력 메소드가 KeyAction안에 존재하는가?
        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
