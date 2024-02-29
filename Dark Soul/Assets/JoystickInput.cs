using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    [Header("===== Joystick Settings =====")]
    public string axisX;
    public string axisY;
    public string axisJright;
    public string axisJup;
    public string btnA;
    public string btnB;
    public string btnX;
    public string btnY;
    public string triggerLR;
    public string btnLB;

    // 按压信号
    public MyButton buttonA = new MyButton();
    public MyButton buttonX = new MyButton();
    public MyButton buttontLR = new MyButton();
    public MyButton buttonLB = new MyButton();

    // Update is called once per frame
    void Update()
    {
        // 激活按压
        buttonA.tickButton(Input.GetButton(btnA));
        buttonX.tickButton(Input.GetButton(btnX));
        buttontLR.tickAxis(Input.GetAxis(triggerLR));
        buttonLB.tickButton(Input.GetButton(btnLB));

        // 摄像机水平、竖直方向设置（取消缓动）(右摇杆)
        DJup = Input.GetAxis(axisJup) * (-1.0f);
        DJright = Input.GetAxis(axisJright);

        // 角色水平、竖直方向设置 （取消缓动）（左摇杆）
        Dup = Input.GetAxis(axisY);
        Dright = Input.GetAxis(axisX);
        //Dmag = new Vector3(Dup, Dright).normalized.magnitude;
        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvec = Dright * transform.right + Dup * transform.forward;

        // 长按
        run = buttontLR.IsPressing;
        // 按下一帧
        jump = buttonA.OnPressed;
        // 攻击动作
        attack = buttonX.OnPressed;
        // 举盾
        defense = buttonLB.IsPressing;

        if (!inputEnabled)
        {
            Dup = 0;
            Dright = 0;
            Dmag = 0;
        }
    }
}
