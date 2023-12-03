using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TIME_CONTROL
{
    public class TweenSelect : MonoBehaviour
    {
        public bool isTrigger;
        // 트리거 조건을 설정하는 변수
        public enum TweenType
        // Tween 애니메이션 유형을 정의하는 열거형
        {
            MOVE = 1,//이동
            ROTATE,//회전
            SCALE_AND_FADE //크기 조절 및 투명도 조절
        }
        public TweenType tweenType; // 선택한 Tween 애니메이션 유형
        public int number; //  Rotate 또는 다른 TweenType의 리스트의 인덱스  값
        private bool hasTriggered = false; //Tween 액션이 실행되었는지 여부를 나타내는 변수
        private bool isTweening = false; // 현재 Tween 액션이 실행 중인지 여부를 나타내는 변수

        private void Update()
        {
            if (isTrigger && !isTweening) 
             //트리거 조건 충족하고 Tween 액션 실행 중이 아닌 경우
            {
                LaunchTween(); //Tween 애니메이션 시작
            }
        }

        private void LaunchTween() // Tween 시작 메서드
        {
            if (hasTriggered || isTweening) return;
            // 이미 Tween 액션이 실행 중이거나 완료된 경우, 더 이상 실행하지 않음
            switch (tweenType)
            {
                case TweenType.MOVE: //이동
                    iTween.MoveBy(gameObject, TweenData.move[number]);
                    // 이동 애니메이션 시작
                    break;
                case TweenType.ROTATE: //회전
                    iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(0, 0, 360), "time", 5.0f, "loopType", "loop"));
                    // 회전 애니메이션 시작
                    break;
                case TweenType.SCALE_AND_FADE: //크기 조절 및 투명도 조절                  
                    iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 2.0f));
                    // 스케일 조절 애니메이션 추가 (예: 2초 동안)
                    iTween.FadeTo(gameObject, 0.0f, 2.0f);
                    // 투명도 조절 애니메이션 추가 (예: 2초 동안)
                    break;
            }
            hasTriggered = true; // Tween 액션이 시작되었음을 표시
        }

        private void OnTweenComplete() //Tween 액션 완료 메서드
        {
            isTweening = false; // 실행 중인 Twwen 액션을 false로 전환
        }

        public void OnCollisionEnter(Collision collision) //충돌 메서드
        {
            if (collision.gameObject.layer == 7)
            // 충돌한 객체의 레이어가 7인 경우
            {
                if (!isTrigger)
                // 트리거가 아닌 경우
                {
                    LaunchTween(); // Tween 애니메이션 시작
                }
            }
        }
    }
}
