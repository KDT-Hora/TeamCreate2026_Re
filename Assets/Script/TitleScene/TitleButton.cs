using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // これが必要

public class TitleButton : MonoBehaviour
{
    void Awake()
    {
        // 画面比率
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        if (DataManager.Instance != null)
        {
            DataManager.Instance.ResetAllData();
        }
        if (FieldData.Instance != null)
        {
            FieldData.Instance.Initialize();
        }
    }
    void Start()
    {
        SoundManager.Instance.PlayBGM("BGM_Title");
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