using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public Sprite defoSprite;
    public Sprite nextSprite;
    private Image image;

    void Start()
    {
        

        {
            image=GetComponent<Image>();
            defoSprite=image.sprite;
        }
    }

    public void OnClick()
    {

    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider)
        {
            image.sprite=nextSprite;
        }
        else
        {
            image.sprite=defoSprite;
        }

    }
}
