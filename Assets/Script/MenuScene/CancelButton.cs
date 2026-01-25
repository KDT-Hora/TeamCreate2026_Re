using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CancelButton : MonoBehaviour
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
        Debug.Log("CancelButton ‰Ÿ‚³‚ê‚½");

        back.SetActive(true);
        book.SetActive(false);
        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        button4.SetActive(true);
        button5.SetActive(false);
        button6.SetActive(true);

        statusPanel.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
