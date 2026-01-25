using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float height = 20f;

    void LateUpdate()
    {
        if (!target) return;

        transform.position = new Vector3(
            target.position.x,
            height,
            target.position.z
        );
    }
}
