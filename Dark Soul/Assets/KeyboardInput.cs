using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    [Header("===== Setting keys =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyJUp = "up";
    public string keyJDown = "down";
    public string keyJLeft = "left";
    public string keyJRight = "right";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    [Header("===== Mouse Settings =====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX;
    public float mouseSensitivityY;

    // Update is called once per frame
    void Update()
    {
        // 摄像机水平、竖直方向设置（取消缓动）
        if (mouseEnable)
        {
            DJup = Input.GetAxis("Mouse Y") * mouseSensitivityX;
            DJright = Input.GetAxis("Mouse X") * mouseSensitivityY;
        }
        else
        {
            DJup = ((Input.GetKey(keyJUp) ? 1.0f : 0.0f) - (Input.GetKey(keyJDown) ? 1.0f : 0.0f));
            DJright = ((Input.GetKey(keyJRight) ? 1.0f : 0.0f) - (Input.GetKey(keyJLeft) ? 1.0f : 0.0f));
        }
        // 角色水平、竖直方向设置 （取消缓动）
        Dup = ((Input.GetKey(keyUp) ? 1.0f : 0.0f) - (Input.GetKey(keyDown) ? 1.0f : 0.0f));
        Dright = ((Input.GetKey(keyRight) ? 1.0f : 0.0f) - (Input.GetKey(keyLeft) ? 1.0f : 0.0f));
        Dmag = new Vector3(Dup, Dright).normalized.magnitude;
        Dvec = Dright * transform.right + Dup * transform.forward;

        // 限制坐标轴（-1，1）
        //targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        //targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        // 缓动
        //Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        //Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f

        // 长按
        run = Input.GetKey(keyA);
        // 按下一帧
        jump = Input.GetKeyDown(keyB);
        // 攻击动作
        attack = Input.GetKeyDown(keyC);
        // 盾牌防御
        defense = Input.GetKey(keyD);

        if (!inputEnabled)
        {
            Dup = 0;
            Dright = 0;
            Dmag = 0;
        }


    }
}
