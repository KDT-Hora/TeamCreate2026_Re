using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // これが必要

public class FieldButton : MonoBehaviour
{
    void Update()
    {
        // スペースキーを押したらバトルシーンへ移動
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FadeManager.FadeChangeScene("MenuScene", 1.0f);
        }
    }

    void Start()
    {
    }
}