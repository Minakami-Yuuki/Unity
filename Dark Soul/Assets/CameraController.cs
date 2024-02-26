using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float cameraDampSpeed;

    private GameObject playHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject chasingCamera;

    private Vector3 cameraDampVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playHandle = cameraHandle.transform.parent.gameObject;
        model = playHandle.GetComponent<ActorController>().model;
        chasingCamera = Camera.main.gameObject;
    }

    // Update is called once per frame (Fixed)
    void FixedUpdate()
    {
        // 保留当前世界欧拉角不变
        Vector3 tempModelEuler = model.transform.eulerAngles;

        playHandle.transform.Rotate(Vector3.up, pi.DJright * horizontalSpeed * Time.fixedDeltaTime);
        // 限制竖直旋转角度为： [-20, 30]
        tempEulerX -= pi.DJup * verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -20, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

        // 结束相机旋转后重新赋予模型更改后的世界欧拉角
        model.transform.eulerAngles = tempModelEuler;

        // 相机追随角色缓动
        chasingCamera.transform.position = Vector3.SmoothDamp(
            chasingCamera.transform.position,
            transform.position,
            ref cameraDampVelocity,
            cameraDampSpeed);
        // 跟随相机旋转后被赋予新世界欧拉角
        chasingCamera.transform.eulerAngles = transform.eulerAngles;
    }
}
