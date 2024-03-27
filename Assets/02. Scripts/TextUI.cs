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
                text.text = "첫번째 문장입니다.\n클릭하면 다음 문장이 나타납니다.";
                clickCount++;
            }
            else if(clickCount==1)
            {
                text.text = "두번째 문장입니다.\n클릭하면 대화창이 사라집니다.";
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
