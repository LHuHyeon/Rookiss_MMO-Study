using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        // recursive : 자식 객체를 가져올지 판단
        if (recursive == false){
            // go의 자식객체 수 만큼
            for(int i=0; i<go.transform.childCount; i++){
                // 지정된 자식객체를 transform에 반환
                Transform transform = go.transform.GetChild(i);

                // string.IsNullOrEmpty = 빈문자열이면 true (null 또는 "")
                if (string.IsNullOrEmpty(name) || transform.name == name){
                    // 해당 T(Button, Text, ...) 컴포넌트 반환
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else{
            // 자식 T 객체 전체 가져오기
            foreach(T component in go.GetComponentsInChildren<T>()){
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
