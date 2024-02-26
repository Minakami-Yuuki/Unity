using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ǽ��
[RequireComponent(typeof(Rigidbody))]
public class ActorController : MonoBehaviour
{

    // ģ��
    public GameObject model;
    public PlayerInput pi;
    // ���ߡ������ٶ�
    public float walkSpeed;
    public float runMultiplier;
    // ��Ծ�߶�
    public float jumpVelocity;
    // �����߶�
    public float rollVelocity;

    // Ħ����
    [Space(10)]
    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;
    public CapsuleCollider col;

    // ����ģ��
    // ����
    [SerializeField]
    private Animator animator;
    // ����
    private Rigidbody rigid;
    // �洢�û�ʱ������
    private Vector3 planarVec;
    // �洢����
    private Vector3 thrustVec;
    // ��ס��ǰƽ��
    private bool lockPlanar = false;
    // ֻ���ڵ����ϲ��ܽ��й���
    private bool canAttack;
    // ��������
    private float lerpTarget;

    // ����
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
        // ���� + ���� ����·���ܲ��� 50% ���ɣ�
        animator.SetFloat("forward", 
            pi.Dmag * Mathf.Lerp(animator.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f) , 0.5f));
        // ��Ծ
        if (pi.jump)
        {
            animator.SetTrigger("jump");
            canAttack = false;
        }
        // ����
        if (rigid.velocity.magnitude > 8.0f)
        {
            animator.SetTrigger("roll");
        }
        // ����
        if (pi.attack && CheckState("ground") && canAttack)
        {
            animator.SetTrigger("attack");
        }
        // ת�� + ���� ���ɿ������ֹͣ����ת�� 30% ���ɣ�
        if (pi.Dmag > 0.1f)
        {
            // ���β�ֵ��������
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }
        // ��¼�û���ʵ���벽��+����
        if (!lockPlanar)
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.8f);
        }
    }

    // Update is called once per frame (50fps)
    void FixedUpdate()
    {
        // ����bot�ƶ�
        //rigid.position += planarVec * Time.fixedDeltaTime;
        // y��������
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    // ����0��ĵ�ǰ״̬
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
        // ����Y�����
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
        // ����Y�����
        thrustVec = new Vector3(0, rollVelocity, 0);
    }

    void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    void OnJabUpdate()
    {
        // ����Z�����
        thrustVec = model.transform.forward * animator.GetFloat("jabVelocity");
    }

    void OnAttack1AhEnter()
    {
        pi.inputEnabled = false;
        lerpTarget = 1.0f;
    }
    void OnAttack1hAUpdate()
    {
        // ����Z�����
        thrustVec = model.transform.forward * animator.GetFloat("attack1hAVelocity");

        //float currentWeight = animator.GetLayerWeight(animator.GetLayerIndex("attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);
        //animator.SetLayerWeight(animator.GetLayerIndex("attack"), currentWeight);

        // ��������
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

        // ��������
        animator.SetLayerWeight(animator.GetLayerIndex("attack"),
            Mathf.Lerp(animator.GetLayerWeight(animator.GetLayerIndex("attack")), lerpTarget, 0.4f));
    }
}
