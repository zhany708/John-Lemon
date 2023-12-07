using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;        //引用玩家的坐标（必须运用public引用）
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;


    void Update()
    {
        //检查从某个点开始的直线路径上是否存在碰撞体，运用射线投射
        if (m_IsPlayerInRange)
        {
            /*从PointOfView到玩家的方向，也就是两点之间的矢量（B-A）
            1.将方向设置为向上的一个单位，Vector3.up是（0,1,0）的快捷方式
            */
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);       //射线
            RaycastHit raycastHit;

            //该方程使用out参数返回信息
            if (Physics.Raycast(ray, out raycastHit))       //out参数，该类型参数的值可以由它们的方法更改或设置，以便由任何调用方使用
            {
                if (raycastHit.collider.transform == player)        //检查射线是否击中玩家
                {
                    gameEnding.CaughtPlayer();      //调用Setter函数
                }
            }
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)      //检测触发器是否触发玩家坐标
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)      //检测玩家何时离开触发器
    {
        if (other.transform == player)    
        {
            m_IsPlayerInRange = false;
        }
    }
}