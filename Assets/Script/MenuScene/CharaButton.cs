using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaButton : MonoBehaviour
{
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject book;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;
    [SerializeField] private GameObject button4;
    [SerializeField] private GameObject button5;
    [SerializeField] private GameObject button6;

    public void OnClick()
    {

    }

    public void OnClickBack()
    {
        back.SetActive(false);
        book.SetActive(true);
        text.SetActive(true);
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button5.SetActive(true);
        button6.SetActive(false);
    }

    void Update()
    {

    }
}
