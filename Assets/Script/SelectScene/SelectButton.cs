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
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
        SoundManager.Instance.PlaySE("SE_Confirm");
    }
    public void Syuten()
    {
        if (System.FadeManager.Instance.GetFadeState()) return;
        Debug.Log("Syuten button pressed");
        SelectData.Instance.SetBoss(SelectData.Boss.Syuten);
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
        SoundManager.Instance.PlaySE("SE_Confirm");
    }
    public void Karasu()
    {
        if (System.FadeManager.Instance.GetFadeState()) return;
        Debug.Log("Karasu button pressed");
        SelectData.Instance.SetBoss(SelectData.Boss.Karasu);
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
        SoundManager.Instance.PlaySE("SE_Confirm");
    }

}
