using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

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

    private void Start() {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
    }

    // where T : 부모 클래스의 자식 클래스만 가능
    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // C++과 다르게 C#은 enum안에 있는 내용을 읽을 수 있다!
        string[] names = Enum.GetNames(type);   // C# 기능
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++){
            objects[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    public void OnButtonClicked()
    {
        score++;
    }
}
