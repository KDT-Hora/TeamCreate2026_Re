using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject menuBackground;
    public GameObject menuWindow;

    public void ToggleMenu()
    {
        if (menuWindow.activeSelf)
        {
            menuWindow.SetActive(false);
            menuBackground.SetActive(false);
        }
        else
        {
            menuBackground.SetActive(true);
            menuWindow.SetActive(false);
        }
    }

}
