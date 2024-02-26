using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 标记检查
[RequireComponent(typeof(Rigidbody))]
public class ActorController : MonoBehaviour
{

    // 模型
    public GameObject model;
    public PlayerInput pi;
    // 行走、奔跑速度
    public float walkSpeed;
    public float runMultiplier;
    // 跳跃高度
    public float jumpVelocity;
    // 翻滚高度
    public float rollVelocity;

    // 摩擦力
    [Space(10)]
    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;
    public CapsuleCollider col;

    // 动作模组
    // 动画
    [SerializeField]
    private Animator animator;
    // 刚体
    private Rigidbody rigid;
    // 存储用户时刻输入
    private Vector3 planarVec;
    // 存储冲量
    private Vector3 thrustVec;
    // 锁住当前平面
    private bool lockPlanar = false;
    // 只能在地面上才能进行攻击
    private bool canAttack;
    // 攻击缓动
    private float lerpTarget;

    // 激活
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        animator = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame (60fps)
    void Update()
    {
        // 步长 + 渐变 （走路到跑步的 50% 过渡）
        animator.SetFloat("forward", 
            pi.Dmag * Mathf.Lerp(animator.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f) , 0.5f));
        // 跳跃
        if (pi.jump)
        {
            animator.SetTrigger("jump");
            canAttack = false;
        }
        // 翻滚
        if (rigid.velocity.magnitude > 8.0f)
        {
            animator.SetTrigger("roll");
        }
        // 攻击
        if (pi.attack && CheckState("ground") && canAttack)
        {
            animator.SetTrigger("attack");
        }
        // 转向 + 渐变 （松开方向键停止）（转向 30% 过渡）
        if (pi.Dmag > 0.1f)
        {
            // 球形插值（缓动）
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }
        // 记录用户事实输入步长+方向
        if (!lockPlanar)
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.8f);
        }
    }

    // Update is called once per frame (50fps)
    void FixedUpdate()
    {
        // 更新bot移动
        //rigid.position += planarVec * Time.fixedDeltaTime;
        // y分量不变
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    // 检测第0层的当前状态
    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }


    /// <summary>
    /// Message processing block:
    /// </summary>

    void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        // 赋予Y轴冲量
        thrustVec = new Vector3(0, jumpVelocity, 0);
        //print("Jump Up!!");
    }

    //void OnJumpExit()
    //{
    //    pi.inputEnabled = true;
    //    lockPlanar = false;
    //    print("Jump Down!!");
    //}

    void IsGround()
    {
        animator.SetBool("isGround", true);
    }
    void IsNotGround()
    {
        animator.SetBool("isGround", false);
    }

    void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
    }

    void OnGroundExit()
    {
        col.material = frictionZero;
    }

    void OnFallEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        // 赋予Y轴冲量
        thrustVec = new Vector3(0, rollVelocity, 0);
    }

    void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    void OnJabUpdate()
    {
        // 赋予Z轴冲量
        thrustVec = model.transform.forward * animator.GetFloat("jabVelocity");
    }

    void OnAttack1AhEnter()
    {
        pi.inputEnabled = false;
        lerpTarget = 1.0f;
    }
    void OnAttack1hAUpdate()
    {
        // 赋予Z轴冲量
        thrustVec = model.transform.forward * animator.GetFloat("attack1hAVelocity");

        //float currentWeight = animator.GetLayerWeight(animator.GetLayerIndex("attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);
        //animator.SetLayerWeight(animator.GetLayerIndex("attack"), currentWeight);

        // 攻击缓动
        animator.SetLayerWeight(animator.GetLayerIndex("attack"), 
            Mathf.Lerp(animator.GetLayerWeight(animator.GetLayerIndex("attack")), lerpTarget, 0.4f));
    }
    void OnAttackIdleEnter()
    {
        pi.inputEnabled = true;
        lerpTarget = 0.0f;
    }

    void OnAttackIdleUpdate()
    {
        //float currentWeight = animator.GetLayerWeight(animator.GetLayerIndex("attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);
        //animator.SetLayerWeight(animator.GetLayerIndex("attack"), currentWeight);

        // 攻击缓动
        animator.SetLayerWeight(animator.GetLayerIndex("attack"),
            Mathf.Lerp(animator.GetLayerWeight(animator.GetLayerIndex("attack")), lerpTarget, 0.4f));
    }
}
