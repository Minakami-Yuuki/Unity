using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 碰撞体检测 （胶囊）
public class OnGroundSensor : MonoBehaviour
{

    // 胶囊体
    public CapsuleCollider capcol;
    // 缩减胶囊体大小
    public float offset;

    // Down：胶囊体下侧圆心
    // Up:胶囊体上侧圆心
    // radius：二者相同的半径
    private Vector3 pointDown;
    private Vector3 pointUp;
    private float radius;

    // Start is called before the first frame update
    void Awake()
    {
        // 缩减范围大小
        radius = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 下侧圆心：当前脚底坐标 + 单位向量（方向） * 长度
        pointDown = transform.position + transform.up * (radius - offset);
        // 上侧圆心：当前脚底坐标 + 单位向量（方向） * 胶囊体长度 + 单位向量（方向） * 长度
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
