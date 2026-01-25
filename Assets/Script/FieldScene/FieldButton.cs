using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // ‚±‚ê‚ª•K—v

public class FieldButton : MonoBehaviour
{
    public void ClickToMenu()
    {
        SoundManager.Instance.PlaySE("SE_Confirm");
        FadeManager.FadeChangeScene("MenuScene", 1.0f);
    }

    void Start()
    {
    }
}