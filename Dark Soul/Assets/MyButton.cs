using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
     public bool IsPressing = false;
     public bool OnPressed = false;
     public bool OnReleased = false;
     public bool curState = false;
     public bool lastState = false;

    // Button ��ѹ�ź�
     public void tickButton(bool input)
    {
        
        curState = input;
        IsPressing = curState;

        // ����ۼ�
        OnPressed = false;
        OnReleased = false;
        if(curState != lastState)
        {
            if(curState == true)
            {
                OnPressed = true;
            }
            else
            {
                OnReleased = true;
            }
        }
        lastState = curState;
    }

    public void tickAxis(float input)
    {
        if(input < 0.0f)
        {
            IsPressing = true;
        }
        else
        {
            IsPressing = false;
        }
    }
}    

