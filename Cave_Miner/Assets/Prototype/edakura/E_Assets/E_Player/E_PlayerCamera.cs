using UnityEngine;

public class E_PlayerCamera : MonoBehaviour
{
    public Transform target;     // 追従する対象（プレイヤー）
    public Vector3 offset;       // カメラの位置のずれ（必要に応じて調整）

    void LateUpdate()
    {
        if (target != null)
        {
            // Z軸はカメラの深さなので固定（-10とか）
            Vector3 newPosition = target.position + offset;
            newPosition.z = transform.position.z;

            transform.position = newPosition;
        }
    }
}