using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween_Path : MonoBehaviour
{
    // 플레이어 오브젝트를 참조할 변수
    public GameObject m_PlayerObj;

    // 경로를 정의하는 지점들의 배열
    public Transform[] positionPoint;

    // 0부터 1 사이의 값을 설정할 변수
    [Range(0, 1)]
    public float value;

    // 충돌 여부를 추적하기 위한 변수
    private bool hasCollided = false;

  

    void Update()
    {
        if (hasCollided) // 충돌했을 때만 경로 이동
        {
            // value 값을 1로 증가시키는데 시간에 따라 천천히 증가
            if (value < 1)
            {
                value += Time.deltaTime / 10;
            }

            // 경로 상에서 value 위치에 플레이어 오브젝트를 배치
            iTween.PutOnPath(m_PlayerObj, positionPoint, value);
        }
    }

    // 에디터 상에서 시각적으로 경로를 나타내는 함수
    private void OnDrawGizmos()
    {
        iTween.DrawPath(positionPoint, Color.green);
        //iTween 라이브러리의 DrawPath 함수를 사용하여 positionPoint 배열로 지정된 초록색 경로를 그림
    }

    // 충돌을 감지하는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // 만약 충돌한 오브젝트가 "Touch" 태그를 가지고 있다면 아래 코드를 실행
        if (collision.gameObject.CompareTag("Touch"))
        {
            // 충돌이 감지되면 충돌 플래그를 설정하여 경로 이동을 시작
            hasCollided = true;
        }
    }
}

