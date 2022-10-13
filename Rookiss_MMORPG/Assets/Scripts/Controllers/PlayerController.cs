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
                    anim.SetFloat("speed", _stat.MoveSpeed);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Idle:
                    anim.SetFloat("speed", 0f);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Skill:
                    anim.SetBool("attack", true);
                    break;
                case PlayerState.Die:
                    anim.SetBool("attack", false);
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
        if (_lockTarget != null){
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1f){
                State = PlayerState.Skill;
                return;
            }
        }

        // 도착 위치 벡터에서 플레이어 위치 벡터를 뺀다.
        Vector3 dir = _destPos - transform.position;

        // Vector3.magnitude = 벡터값의 길이
        if (dir.magnitude < 0.1f){
            State = PlayerState.Idle;
        }
        else{
            // TODO
            NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            // 건물을 클릭하여 이동하면 건물 앞에 멈추기 (1.0f 거리에서 멈추기)
            Debug.DrawRay(transform.position + (Vector3.up * 0.5f), dir.normalized, Color.red);
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), dir, 1.0f, LayerMask.GetMask("Block"))){
                if (Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;
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
    }

    // 죽음 메소드
    void UpdateDie()
    {
        // 아무것도 못함.
    }

    // 애니메이션에서 이벤트 호출
    void OnHitEvent()
    {
        State = PlayerState.Idle;
    }
    
    // 마우스 클릭 메소드
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == PlayerState.Die)
            return;

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
                    if (_lockTarget != null)
                        _destPos = _lockTarget.transform.position;
                    else if (raycastHit) {
                        _destPos = hit.point;
                    }
                }
                break;
            // // 마우스를 땠을 때 (한번 클릭만 했을 때 타겟팅 되면 타겟이 null되므로 삭제.)
            // case Define.MouseEvent.PointUp:
            //     _lockTarget = null;
            //     break;
        }
    }
}
