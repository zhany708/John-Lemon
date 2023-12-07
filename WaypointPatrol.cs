using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;       //引用导航网格代理
    public Transform[] waypoints;           //包含坐标的数组

    int m_CurrentWaypointIndex;


    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);     //初始化导航网格代理
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)     //查看到目标的剩余距离是否小于设置的停止距离
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;       //如果加一后导致索引等于数组中元素的数量，重置索引为0。因为任何数字除以本身时，余数都为0

            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);        //使用当前路径点作为索引
        }
    }
}
