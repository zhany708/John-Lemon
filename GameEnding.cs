using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;     // 淡入淡出的持续时间
    public float displayImageDuration = 1f;     // 额外的图像显示时间
    public GameObject player;       // 引用游戏角色
    public CanvasGroup exitBackgroundImageCanvasGroup;      //引用玩家赢得游戏后显示的蓝图
    public CanvasGroup caughtBackgroundImageCanvasGroup;    //引用玩家被抓住后显示的蓝图
    public AudioSource exitAudio;       //游戏胜利音效
    public AudioSource caughtAudio;     //游戏失败音效


    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    bool m_HasAudioPlayed;      //确保音效只放一次
    float m_Timer;      // 计时器



    void Update()
    {
        if (m_IsPlayerAtExit)       //玩家逃脱
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }

        else if (m_IsPlayerCaught)  //玩家被抓住
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }



    void OnTriggerEnter(Collider other)     
    {
        if (other.gameObject == player)     // 检查角色是否触发碰撞
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()      //Setter函数
    {
        m_IsPlayerCaught = true;
    }


    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)     // 使Canvas Group组件淡入淡出，然后退出游戏
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }


        m_Timer += Time.deltaTime;      // 计时器增加从上一帧以来经过的时间
        imageCanvasGroup.alpha = m_Timer/fadeDuration;        // 计时器为0时，Alpha为0.计时器不超过fadeDuration时，Alpha为1

        if (m_Timer > fadeDuration + displayImageDuration)      // 给玩家多一点看游戏结束图像的时间
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);      //回到场景的构造索引为0的时候（也就是重新开始游戏）
            }

            else
            {
                Application.Quit();     // 退出游戏
            }
        }
    }
}