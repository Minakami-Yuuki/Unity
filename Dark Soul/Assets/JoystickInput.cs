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

    // ��ѹ�ź�
    public MyButton buttonA = new MyButton();
    public MyButton buttonX = new MyButton();
    public MyButton buttontLR = new MyButton();
    public MyButton buttonLB = new MyButton();

    // Update is called once per frame
    void Update()
    {
        // ���ѹ
        buttonA.tickButton(Input.GetButton(btnA));
        buttonX.tickButton(Input.GetButton(btnX));
        buttontLR.tickAxis(Input.GetAxis(triggerLR));
        buttonLB.tickButton(Input.GetButton(btnLB));

        // �����ˮƽ����ֱ�������ã�ȡ��������(��ҡ��)
        DJup = Input.GetAxis(axisJup) * (-1.0f);
        DJright = Input.GetAxis(axisJright);

        // ��ɫˮƽ����ֱ�������� ��ȡ������������ҡ�ˣ�
        Dup = Input.GetAxis(axisY);
        Dright = Input.GetAxis(axisX);
        //Dmag = new Vector3(Dup, Dright).normalized.magnitude;
        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvec = Dright * transform.right + Dup * transform.forward;

        // ����
        run = buttontLR.IsPressing;
        // ����һ֡
        jump = buttonA.OnPressed;
        // ��������
        attack = buttonX.OnPressed;
        // �ٶ�
        defense = buttonLB.IsPressing;

        if (!inputEnabled)
        {
            Dup = 0;
            Dright = 0;
            Dmag = 0;
        }
    }
}
