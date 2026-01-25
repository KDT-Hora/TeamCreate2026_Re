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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            back.SetActive(true);
            book.SetActive(false);
            text.SetActive(false);
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
        }
    }
}
