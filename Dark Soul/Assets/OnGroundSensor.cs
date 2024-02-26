using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ײ���� �����ң�
public class OnGroundSensor : MonoBehaviour
{

    // ������
    public CapsuleCollider capcol;
    // �����������С
    public float offset;

    // Down���������²�Բ��
    // Up:�������ϲ�Բ��
    // radius��������ͬ�İ뾶
    private Vector3 pointDown;
    private Vector3 pointUp;
    private float radius;

    // Start is called before the first frame update
    void Awake()
    {
        // ������Χ��С
        radius = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // �²�Բ�ģ���ǰ�ŵ����� + ��λ���������� * ����
        pointDown = transform.position + transform.up * (radius - offset);
        // �ϲ�Բ�ģ���ǰ�ŵ����� + ��λ���������� * �����峤�� + ��λ���������� * ����
        pointUp = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;

        Collider[] outputCols = Physics.OverlapCapsule(pointDown, pointUp, radius, LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");

            //foreach (var col in outputCols)
            //{
            //    print("collision: " + col.name);
            //}
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
