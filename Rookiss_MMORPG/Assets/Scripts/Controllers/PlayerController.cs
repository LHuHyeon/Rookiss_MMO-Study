using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;   // 플레이어 스탯
    Vector3 _destPos;   // 도착 좌표

    RaycastHit hit;
    Animator anim;

    GameObject _lockTarget; // 마우스로 타겟한 오브젝트 담는 변수

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    // 플레이어 상태
    public enum PlayerState
    {
        Moving,
        Idle,
        Die,
        Skill,
    }
    [SerializeField]
    PlayerState _state = PlayerState.Idle;

    // 플레이어 상태에 따라 애니메이션이 작동하는 _state의 프로퍼티
    public PlayerState State
    {
        get { return _state; }
        set {
            _state = value;

            switch (_state){
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);    // CrossFade는 애니메이션의 부드러움, 반복도 등 설정이 가능하다.
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
                case PlayerState.Die:
                    break;
            }
        }
    }

    void Start()
    {
        _stat = gameObject.GetComponent<PlayerStat>();
        anim = GetComponent<Animator>();

        // OnKeyboard를 빼준 후 더해주는 이유
        // 같은 메소드가 두번 호출되는 것을 막기 위해서.
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    void Update()
    {
        // State 패턴
        switch (State){
            case PlayerState.Moving:    // 움직임
                UpdateMoving();
                break;
            case PlayerState.Idle:      // 가만히 있기
                UpdateIdle();
                break;
            case PlayerState.Skill:     // 스킬
                UpdateSkill();
                break;
            case PlayerState.Die:       // 죽음
                UpdateDie();
                break;
        }
    }

    // 이동 메소드
    void UpdateMoving()
    {
        // 타겟(몬스터)가 존재하면 두 사이 거리가 1f 일때 멈추고 스킬 시전(공격)
        if (_lockTarget != null){
            float distance = (_lockTarget.transform.position - transform.position).magnitude;
            if (distance <= 1f){
                Debug.Log("Skill On");
                State = PlayerState.Skill;
                return;
            }
        }

        // 타겟 대상을 클릭했을 때 콜라이더 위쪽을 클릭하게 된다면 그쪽을 바라보고 달리기 때문에 y값을 0으로 준다.
        _destPos.y = 0; 

        // 도착 위치 벡터에서 플레이어 위치 벡터를 뺀다.
        Vector3 dir = _destPos - transform.position;

        // Vector3.magnitude = 벡터값의 길이
        if (dir.magnitude < 0.1f){
            State = PlayerState.Idle;
        }
        else{
            NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            // 건물을 클릭하여 이동하면 건물 앞에 멈추기 (1.0f 거리에서 멈추기)
            Debug.DrawRay(transform.position + (Vector3.up * 0.5f), dir.normalized, Color.red);
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), dir, 1.0f, LayerMask.GetMask("Block"))){
                if (Input.GetMouseButton(0) == false){
                    State = PlayerState.Idle;
                }
                return;
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);
        }
    }
    
    // 멈추는 메소드
    void UpdateIdle()
    {
    }

    void UpdateSkill()
    {
        // 타겟 바라보기
        if (_lockTarget != null){
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    // 죽음 메소드
    void UpdateDie()
    {
        // 아무것도 못함.
    }

    // 애니메이션에서 이벤트 호출 (공격 모션 끝날 쯤 실행 됨.)
    void OnHitEvent()
    {
        // TODO
        if (_stopSkill){
            State = PlayerState.Idle;
        }
        else{
            State = PlayerState.Skill;
        }
    }
    
    // 마우스 클릭 메소드
    bool _stopSkill = false;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch(State){
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                {
                    if (evt == Define.MouseEvent.PointUp){  // 클릭을 때면 공격이 끝났다는 뜻이므로 스킬 중지
                        _stopSkill = true;
                    }
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        // 메인 카메라에서 마우스가 가르키는 위치의 ray를 저장
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100f, _mask);

        switch (evt)
        {
            // 마우스를 클릭했을 때 [ 클릭한 위치로 이동 ]
            case Define.MouseEvent.PointDown:
                {
                    if (raycastHit){
                        _destPos = hit.point;   // 해당 좌표 저장
                        State = PlayerState.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            // 마우스를 클릭 중일 때
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget != null && raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointUp:
                _stopSkill = true;
                break;
        }
    }
}
