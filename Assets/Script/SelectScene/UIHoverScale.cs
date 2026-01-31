using UnityEngine;

public class UIHoverScale : MonoBehaviour
{
    public float hoverScale = 1.1f;
    public float speed = 10f;

    Vector3 defaultScale;
    Vector3 targetScale;

    void Awake()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );
    }

    public void OnEnter()
    {
        targetScale = defaultScale * hoverScale;
    }

    public void OnExit()
    {
        targetScale = defaultScale;
    }
}
