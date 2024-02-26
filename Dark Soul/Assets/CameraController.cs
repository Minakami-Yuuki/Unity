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
        // ������ǰ����ŷ���ǲ���
        Vector3 tempModelEuler = model.transform.eulerAngles;

        playHandle.transform.Rotate(Vector3.up, pi.DJright * horizontalSpeed * Time.fixedDeltaTime);
        // ������ֱ��ת�Ƕ�Ϊ�� [-20, 30]
        tempEulerX -= pi.DJup * verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -20, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

        // ���������ת�����¸���ģ�͸��ĺ������ŷ����
        model.transform.eulerAngles = tempModelEuler;

        // ���׷���ɫ����
        chasingCamera.transform.position = Vector3.SmoothDamp(
            chasingCamera.transform.position,
            transform.position,
            ref cameraDampVelocity,
            cameraDampSpeed);
        // ���������ת�󱻸���������ŷ����
        chasingCamera.transform.eulerAngles = transform.eulerAngles;
    }
}
