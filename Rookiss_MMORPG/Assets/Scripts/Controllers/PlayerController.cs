using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;   // 이동 속도
    
    Vector3 _destPos;               // 도착 좌표

    RaycastHit hit;
    Animator anim;

    // 플레이어 상태
    public enum PlayerState
    {
        Moving,
        Idle,
        Die,
    }
    PlayerState _state = PlayerState.Idle;

    void Start()
    {
        anim = GetComponent<Animator>();

        // OnKeyboard를 빼준 후 더해주는 이유
        // 같은 메소드가 두번 호출되는 것을 막기 위해서.
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        // TEMP
        Managers.UI.ShowSceneUI<UI_Inven>();
    }

    void Update()
    {
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

    // 이동 메소드
    void UpdateMoving()
    {
        // 도착 위치 벡터에서 플레이어 위치 벡터를 뺀다.
        Vector3 dir = _destPos - transform.position;
        Quaternion dr;

        // Vector3.magnitude = 벡터값의 길이
        if (dir.magnitude < 0.0001f){
            _state = PlayerState.Idle;
        }
        else{
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            dr = Quaternion.LookRotation(dir);
            dr.x = 0;   // 위치 도착 후 앞으로 인사하는 행동(자꾸 앞으로 기울임) 방지
            dr.z = 0;
            // ↓ 부드럽게 해당 위치 바라보기
            transform.rotation = Quaternion.Slerp(transform.rotation, dr, 20f * Time.deltaTime);
            // transform.LookAt(_destPos); // 해당 위치를 바라보기 (딱딱함)   
        }

        // 애니메이션
        anim.SetFloat("speed", _speed);
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
    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        // 메인 카메라에서 마우스가 가르키는 위치의 ray를 저장
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 씬에서 ray가 출력
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        // Ray를 발사한 위치에 정보를 가져온다.
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Wall"))){
            _destPos = hit.point;   // 해당 좌표 저장
            _state = PlayerState.Moving;
        }
    }
}
