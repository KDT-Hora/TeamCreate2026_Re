using System;
using Unity.VisualScripting;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickReturnField();
        }
    }
    /// <summary>
    /// エスケープでメニューからフィールドに戻るボタン
    /// </summary>
    public void OnClickReturnField()
    {
        FadeManager.FadeChangeScene("FieldScene", 1.0f);
    }

    /// <summary>
    /// タイトルに戻るボタン
    /// </summary>
    public void OnClickReturnTitle()
    {
        FadeManager.FadeChangeScene("TitleScene", 1.0f);
    }
}
