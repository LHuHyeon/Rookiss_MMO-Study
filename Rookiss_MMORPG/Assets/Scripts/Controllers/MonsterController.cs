using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    Stat _stat;   // 몬스터 스탯

    public override void Init()
    {
        _stat = gameObject.GetComponent<Stat>();
        anim = GetComponent<Animator>();

        if (gameObject.GetComponentsInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);  // 체력바 생성
    }

    protected override void UpdateIdle()
    {
        Debug.Log("Monster Idle");
    }

    protected override void UpdateMoving()
    {
        Debug.Log("Monster Moving");
    }

    protected override void UpdateSkill()
    {
        Debug.Log("Monster Skill");
    }

    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent");
    }
}
