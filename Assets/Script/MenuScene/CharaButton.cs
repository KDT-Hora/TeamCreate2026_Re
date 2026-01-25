using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaButton : MonoBehaviour
{
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject book;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button4;
    [SerializeField] private GameObject button5;
    [SerializeField] private GameObject button6;
    [SerializeField] private StatusMenuPanel statusPanel;

    public void OnClick()
    {

    }

    public void OnClickBack()
    {
        Debug.Log("CharaButton âüÇ≥ÇÍÇΩ");

        back.SetActive(false);
        book.SetActive(true);
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button5.SetActive(true);
        button6.SetActive(false);

        Debug.Log($"ÉvÉåÉCÉÑÅ[êî {DataManager.Instance.currentParty.members.Count}");
        if (DataManager.Instance.currentParty.members.Count == 0) return;
        statusPanel.ShowParty(DataManager.Instance.currentParty.members);
    }

    void Update()
    {

    }
}
