using TMPro;
using UnityEngine;

public class nameUI : MonoBehaviour
{
    [Header("追従させたいターゲット")]
    public Transform target;

    [Header("頭上に置くオフセット")]
    public Vector3 offset = new Vector3(0, 2.0f, 0);


    RectTransform rectTransform;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();


    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        transform.position = target.position + offset;

        //  向きをあわせる
        Vector3 lookPos = Camera.main.transform.position;
        lookPos.y = transform.position.y; // 水平だけ向ける
        transform.LookAt(lookPos);

        transform.rotation = Quaternion.Euler(0,
           transform.rotation.eulerAngles.y + 180,
           0);

        //   transform.LookAt(Camera.main.transform);
    }
}
