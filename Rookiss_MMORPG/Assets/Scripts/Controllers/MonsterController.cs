using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    [SerializeField]
    float _scanRange = 10f;     // 주변 탐색 거리

    [SerializeField]
    float _attackRange = 2f;    // 공격 거리

    float distance;     // 타겟과 나의 거리

    Stat _stat;   // 몬스터 스탯
    NavMeshAgent nav;

    public override void Init()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        _stat = gameObject.GetComponent<Stat>();
        anim = GetComponent<Animator>();

        if (gameObject.GetComponentsInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);  // 체력바 생성
    }

    // 주변 플레이어 탐색
    protected override void UpdateIdle()
    {
        // TODO : 매니저가 생기면 옮기자
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        distance = TargetDistance(player);
        if (distance <= _scanRange){
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    // 플레이어에게 접근
    protected override void UpdateMoving()
    {
        // 타겟(플레이어)이 존재하면 두 사이 거리가 _attackRange보다 작거나같을때 멈추고 스킬 시전(공격)
        if (_lockTarget != null){
            distance = TargetDistance(_lockTarget);
            if (distance <= _attackRange){
                nav.SetDestination(transform.position); // 타겟을 나로 지정하면 멈춘다.
                State = Define.State.Skill;
                return;
            }
        }

        // 타겟 대상을 클릭했을 때 콜라이더 위쪽을 클릭하게 된다면 그쪽을 바라보고 달리기 때문에 y값을 0으로 준다.
        _destPos.y = 0; 

        // 도착 위치 벡터에서 플레이어 위치 벡터를 뺀다.
        Vector3 dir = _destPos - transform.position;

        // Vector3.magnitude = 벡터값의 길이
        if (dir.magnitude < 0.1f)
            State = Define.State.Idle;
        else{
            nav.speed = _stat.MoveSpeed;
            nav.SetDestination(_destPos);   // 타겟에 접근
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);
        }
    }

    // 플레이어 공격
    protected override void UpdateSkill()
    {
        // 스킬 사용 중에 타겟 바라보기
        if (_lockTarget != null){
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    // 애니메이션 event
    void OnHitEvent()
    {
        if (_lockTarget != null){
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            Stat myStat = gameObject.GetComponent<Stat>();

            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            targetStat.Hp -= damage;

            if (targetStat.Hp > 0){
                distance = TargetDistance(_lockTarget);
                if (distance <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
                State = Define.State.Idle;
        }
        else
            State = Define.State.Idle;
    }

    // 타겟과 나의 거리
    float TargetDistance(GameObject target)
    {
        _destPos = target.transform.position;
        return (_destPos - transform.position).magnitude;
    }
}
