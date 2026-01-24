using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // これが必要

public class TitleButton : MonoBehaviour
{
    void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM("BGM_Title");
        }
    }

    void Update()
    {
        // スペースキーを押したらバトルシーンへ移動
        if (Input.anyKeyDown)
        {
            FadeManager.FadeChangeScene("SelectScene", 1.0f);
        }
    }
}