using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Define.CameraMode _mode = Define.CameraMode.QuarterView;
    [SerializeField]
    private Vector3 _delta;
    [SerializeField]
    private GameObject _player;

    void Start()
    {
        
    }

    // 카메라 위치 이동을 마지막 업데이트에 실행함으로 써 떨림현상 완화
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView){
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform);
        }
    }

    // 카메라 위치 메소드
    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
