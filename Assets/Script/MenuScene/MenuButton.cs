using System;
using Unity.VisualScripting;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MessagePopup popup;
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
        if (System.FadeManager.Instance.GetFadeState()) return;
        SoundManager.Instance.PlaySE("SE_Confirm");
        FadeManager.FadeChangeScene("FieldScene", 1.0f);
    }

    /// <summary>
    /// タイトルに戻るボタン
    /// </summary>
    public void OnClickReturnTitle()
    {
        if (System.FadeManager.Instance.GetFadeState()) return;
        SoundManager.Instance.PlaySE("SE_Confirm");
        FadeManager.FadeChangeScene("TitleScene", 1.0f);
    }

    /// <summary>
    /// クリックされたタイミングで呼ばれる関数
    /// </summary>
    public void OnClick()
    {
        if (System.FadeManager.Instance.GetFadeState()) return;
        SoundManager.Instance.PlaySE("SE_Confirm");
    }
    // 実装待ちテキスト表示
    public void OnClickNotImplemented()
    {
        popup.Show("Waiting for implementation");
    }
}
