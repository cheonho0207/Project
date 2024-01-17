using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TIME_CONTROL
{
    public static class TweenData
    {
        public static List<Hashtable> move, rotate, scale;
        static TweenData()
        {
            InitHashtable();
        }
        private static void InitHashtable()
        {
            InitHashMove();
            InitHashRotate();
            InitHashScale();

        }
        private static void InitHashMove()
        {
            move = null;
            move = new List<Hashtable>(10);
            move.Add(iTween.Hash("y", 8.0, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 0.0, "time", 2.0));
            move.Add(iTween.Hash("x", 5.0, "easeType", "easeInOutSine", "loopType", "pingPong", "delay", 0.0, "time", 2.0));
            move.Add(iTween.Hash("x", 10.0, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 0.0, "time", 2.0));
            move.Add(iTween.Hash("x", -10.0, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 0.0, "time", 2.0));
        }

        private static void InitHashRotate()
        {
            rotate = null;
            rotate = new List<Hashtable>(10);
            move.Add(iTween.Hash("z", 0.5, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 1.0, "time", 1.5));
            move.Add(iTween.Hash("z", -0.5, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 1.0, "time", 1.5));
            move.Add(iTween.Hash("z", 0.5, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 1.0, "time", 1.5));
           
        }

        private static void InitHashScale()
        // 스케일 애니메이션 해시 테이블 초기화
        {
            scale = null;
            scale = new List<Hashtable>(10);
            // 스케일 애니메이션 해시 테이블 리스트 초기화

            move.Add(iTween.Hash("x", 3.0, "easeType", "easeInOutSine", "loopType", "pingPong", "delay", 1.0, "time", 1.5));
            move.Add(iTween.Hash("x", 5.0, "easeType", "easeInOutSine", "loopType", "pingPong", "delay", 0.0, "time", 1.5));
            scale.Add(iTween.Hash("y", 2.0, "easeType", "easeInOutSine", "loopType", "pingPong", "delay", 0.0, "time", 1.5));
            // 여러 가지 스케일 애니메이션 해시 테이블 추가


        }

    }
}