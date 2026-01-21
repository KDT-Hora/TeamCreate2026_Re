using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController1 : MonoBehaviour
{
    [SerializeField] GameObject MenuObject;

    bool menuCondition;

    // Update is called once per frame
    void Update()
    {
        if (menuCondition == false)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                MenuObject.gameObject.SetActive(true);
                menuCondition=true;

                Cursor.visible = true;
                Cursor.lockState=CursorLockMode.None;
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                MenuObject.gameObject.SetActive(false);
                menuCondition=false;

                Cursor.visible=false;
                Cursor.lockState=CursorLockMode.Locked;
            }
        }
        
    }
}
