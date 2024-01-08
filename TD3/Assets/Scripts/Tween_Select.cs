using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TIME_CONTROL
{
    public class TweenSelect : MonoBehaviour
    {
        public bool isTrigger;
        // Ʈ���� ������ �����ϴ� ����
        public enum TweenType
        // Tween �ִϸ��̼� ������ �����ϴ� ������
        {
            MOVE = 1,//�̵�
            ROTATE,//ȸ��
            SCALE_AND_FADE //ũ�� ���� �� ���� ����
        }
        public TweenType tweenType; // ������ Tween �ִϸ��̼� ����
        public int number; //  Rotate �Ǵ� �ٸ� TweenType�� ����Ʈ�� �ε���  ��
        private bool hasTriggered = false; //Tween �׼��� ����Ǿ����� ���θ� ��Ÿ���� ����
        private bool isTweening = false; // ���� Tween �׼��� ���� ������ ���θ� ��Ÿ���� ����

        private void Update()
        {
            if (isTrigger && !isTweening) 
             //Ʈ���� ���� �����ϰ� Tween �׼� ���� ���� �ƴ� ���
            {
                LaunchTween(); //Tween �ִϸ��̼� ����
            }
        }

        private void LaunchTween() // Tween ���� �޼���
        {
            if (hasTriggered || isTweening) return;
            // �̹� Tween �׼��� ���� ���̰ų� �Ϸ�� ���, �� �̻� �������� ����
            switch (tweenType)
            {
                case TweenType.MOVE: //�̵�
                    iTween.MoveBy(gameObject, TweenData.move[number]);
                    // �̵� �ִϸ��̼� ����
                    break;
                case TweenType.ROTATE: //ȸ��
                    iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(0, 0, 360), "time", 5.0f, "loopType", "loop"));
                    // ȸ�� �ִϸ��̼� ����
                    break;
                case TweenType.SCALE_AND_FADE: //ũ�� ���� �� ���� ����                  
                    iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 2.0f));
                    // ������ ���� �ִϸ��̼� �߰� (��: 2�� ����)
                    iTween.FadeTo(gameObject, 0.0f, 2.0f);
                    // ���� ���� �ִϸ��̼� �߰� (��: 2�� ����)
                    break;
            }
            hasTriggered = true; // Tween �׼��� ���۵Ǿ����� ǥ��
        }

        private void OnTweenComplete() //Tween �׼� �Ϸ� �޼���
        {
            isTweening = false; // ���� ���� Twwen �׼��� false�� ��ȯ
        }

        public void OnCollisionEnter(Collision collision) //�浹 �޼���
        {
            if (collision.gameObject.layer == 7)
            // �浹�� ��ü�� ���̾ 7�� ���
            {
                if (!isTrigger)
                // Ʈ���Ű� �ƴ� ���
                {
                    LaunchTween(); // Tween �ִϸ��̼� ����
                }
            }
        }
    }
}
