using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("OnClick()");
    }
    public void Ittan()
    {
        if(System.FadeManager.Instance.GetFadeState()) return;
        Debug.Log("Ittan button pressed");
        SelectData.Instance.SetBoss(SelectData.Boss.Ittan);
        SoundManager.Instance.PlaySE("SE_Confirm");
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
    }
    public void Syuten()
    {
        if (System.FadeManager.Instance.GetFadeState()) return;
        Debug.Log("Syuten button pressed");
        SelectData.Instance.SetBoss(SelectData.Boss.Syuten);
        SoundManager.Instance.PlaySE("SE_Confirm");
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
    }
    public void Karasu()
    {
        if (System.FadeManager.Instance.GetFadeState()) return;
        Debug.Log("Karasu button pressed");
        SelectData.Instance.SetBoss(SelectData.Boss.Karasu);
        SoundManager.Instance.PlaySE("SE_Confirm");
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
    }

}
