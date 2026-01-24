using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Sprite defoSprite;
    public Sprite nextSprite;
    public RectTransform targetImageRect;
    private Image image;

    void Start()
    {
        Button button;

        {
            button = GetComponent<Button>();
            image=GetComponent<Image>();
            
        }
    }

    public void OnClick()
    {

    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            Debug.Log(hit.point);
        }
        else
        {
            image.sprite=defoSprite;
        }
    }
}
