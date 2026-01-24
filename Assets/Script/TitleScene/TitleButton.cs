using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // これが必要

public class TitleButton : MonoBehaviour
{
    void Update()
    {
        // スペースキーを押したらバトルシーンへ移動
        if (Input.anyKeyDown)
        {
            FadeManager.FadeChangeScene("FieldScene", 1.0f);
        }
    }

    void Start()
    {
    }
}