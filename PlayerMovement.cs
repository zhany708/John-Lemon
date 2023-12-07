using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;       // 3表示3弧度，意味着角色转一圈需要一秒。同理，6为半秒
    
    Animator m_Animator;    // 引用动画组件
    Rigidbody m_Rigidbody;  // 引用Rigidbody组件
    AudioSource m_AudioSource;      //引用玩家的脚步声组件
    Vector3 m_Movement;     // 创建矢量
    Quaternion m_Rotation = Quaternion.identity;    // 四元数（Quaternion）是存储旋转的一种方式，identity是为其赋予无旋转的值（初始化）

    // Start is called before the first frame update
    void Start()
    {
        //由于这些变量是private，所以只能在这里分配变量，不能在Unity界面里分配
        m_Animator = GetComponent<Animator>();          //设置Animator组件
        m_Rigidbody = GetComponent<Rigidbody>();        //设置Rigidbody组件
        m_AudioSource = GetComponent<AudioSource>();    //设置脚步声组件
    }

    // 相比Update，此函数按固定频率执行，而不是按帧数。默认情况下每秒50次
    void FixedUpdate()
    {
        float horizontial = Input.GetAxis("Horizontal");        // 左和右（A, D）
        float vertical = Input.GetAxis("Vertical");             // 前和后（W, S）

        m_Movement.Set(horizontial, 0f, vertical);              // 组合上面两个浮点成矢量（Y轴为0）
        m_Movement.Normalize();                                 // 标准化矢量，确保移动矢量始终有相同的大小（防止对角移动比单个轴快）

        // 识别玩家是否有输入
        bool hasHorizontalInput = !Mathf.Approximately(horizontial, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);     // 使用创建的Animator组件引用来调用SetBool, 从而设置 isWalking Animator参数

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)       //确保音源尚未处于播放状态
            {
            m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }


        /* 计算前向矢量
        1. transform.forward是访问Transform组件并获取前向矢量的快捷方式
        2. 后两个参数是起始矢量和目标矢量之间的变化值：首先是角度变化（弧度），然后是大小变化
        3. Time.deltaTime是距上一帧的时间。将每秒所需的更改乘以一帧花费的时间，防止每秒的帧数影响角色的旋转速度
        */
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);       // 在给定参数方向上创建旋转
    }


    // 此函数将通过物理适时被调用，而不是像Update一样通过渲染被调用
    void OnAnimatorMove()       // 更改从Animator中应用根运动的方式（因为实际只需要动画的一部分跟运动）
    {
        // 当前位置加上一移动矢量乘以Animator的deltaPosition的大小，deltaPosition是应用于此帧的跟运动导致的位置变化，将其大小乘以希望的角色移动的实际方向上的移动向量
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);       // 应用移动

        m_Rigidbody.MoveRotation(m_Rotation);       // 应用旋转
    }
}