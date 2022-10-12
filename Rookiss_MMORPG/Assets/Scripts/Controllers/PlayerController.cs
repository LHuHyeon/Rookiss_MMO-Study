using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3 _destPos;               // 도착 좌표

    RaycastHit hit;
    Animator anim;

    Texture2D _attackIcon;
    Texture2D _handIcon;

    // 마우스 커서 상태
    public enum CursorType
    {
        None,
        Attack,
        Hand,
    }
    CursorType _cursorType = CursorType.None;

    // 플레이어 상태
    public enum PlayerState
    {
        Moving,
        Idle,
        Die,
        Skill,
    }
    PlayerState _state = PlayerState.Idle;

    void Start()
    {
        _stat = gameObject.GetComponent<PlayerStat>();
        anim = GetComponent<Animator>();

        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        // OnKeyboard를 빼준 후 더해주는 이유
        // 같은 메소드가 두번 호출되는 것을 막기 위해서.
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        UpdateMouseCursor();

        // State 패턴
        switch (_state){
            case PlayerState.Moving:    // 움직임
                UpdateMoving();
                break;
            case PlayerState.Idle:      // 가만히 있기
                UpdateIdle();
                break;
            case PlayerState.Die:       // 죽음
                UpdateDie();
                break;
        }
    }

    void UpdateMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, _mask)){
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster){
                if (_cursorType != CursorType.Attack){
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else{
                if (_cursorType != CursorType.Hand){
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }

    // 이동 메소드
    void UpdateMoving()
    {
        // 도착 위치 벡터에서 플레이어 위치 벡터를 뺀다.
        Vector3 dir = _destPos - transform.position;

        // Vector3.magnitude = 벡터값의 길이
        if (dir.magnitude < 0.1f){
            _state = PlayerState.Idle;
        }
        else{
            // TODO
            NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            // 건물을 클릭하여 이동하면 건물 앞에 멈추기 (1.0f 거리에서 멈추기)
            Debug.DrawRay(transform.position + (Vector3.up * 0.5f), dir.normalized, Color.red);
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), dir, 1.0f, LayerMask.GetMask("Block"))){
                _state = PlayerState.Idle;
                return;
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);
            
            // // transform.position += dir.normalized * moveDist;
            // dr = Quaternion.LookRotation(dir);
            // dr.x = 0;   // 위치 도착 후 앞으로 인사하는 행동(자꾸 앞으로 기울임) 방지
            // dr.z = 0;
            // // ↓ 부드럽게 해당 위치 바라보기
            // transform.rotation = Quaternion.Slerp(transform.rotation, dr, 20f * Time.deltaTime);
        }

        // 애니메이션
        anim.SetFloat("speed", _stat.MoveSpeed);
    }
    
    // 멈추는 메소드
    void UpdateIdle()
    {
        // 애니메이션
        anim.SetFloat("speed", 0f);
    }

    // 죽음 메소드
    void UpdateDie()
    {
        // 아무것도 못함.
    }
    
    // 마우스 클릭 메소드
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        // 메인 카메라에서 마우스가 가르키는 위치의 ray를 저장
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 씬에서 ray가 출력
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        // Ray를 발사한 위치에 정보를 가져온다.
        if (Physics.Raycast(ray, out hit, 100f, _mask)){
            _destPos = hit.point;   // 해당 좌표 저장
            _state = PlayerState.Moving;

            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster){
                Debug.Log("Monster Click!!");
            }
        }
    }
}
