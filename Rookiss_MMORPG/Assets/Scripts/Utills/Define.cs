using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    // 캐릭터 상태
    public enum State
    {
        Moving,
        Idle,
        Die,
        Skill,
    }

    public enum Layer
    {
        Monster = 6,
        Ground = 7,
        Block = 9,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Loby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,   // MaxCount를 마지막 자리에 둠으로 써 해당 enum의 최대 개수(int)가 저장됨.
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
    
    public enum MouseEvent
    {
        Press,      // 꾹 누를 때 상태
        PointDown,
        PointUp,
        Click,      // 클릭 상태 (Press 상태가 끝난 상태)
    }

    public enum CameraMode
    {
        QuarterView,    // 디아블로 게임 같은 시점
    }
}
