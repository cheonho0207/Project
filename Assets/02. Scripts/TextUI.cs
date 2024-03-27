using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
public GameObject talkPanel;
public Text text;
int clickCount=0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(clickCount==0)
            {
                text.text = "ù��° �����Դϴ�.\nŬ���ϸ� ���� ������ ��Ÿ���ϴ�.";
                clickCount++;
            }
            else if(clickCount==1)
            {
                text.text = "�ι�° �����Դϴ�.\nŬ���ϸ� ��ȭâ�� ������ϴ�.";
                clickCount++;
            }
            else if(clickCount==2)
            {
                talkPanel.SetActive(false);
            }

            Debug.Log(clickCount);
        }
    }
}
