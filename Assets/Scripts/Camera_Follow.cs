using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 5f;
    public float rotateSpeed = 200f;
    float yaw;

    public bool isPlay;
    void LateUpdate()
    {
        if (isPlay)
        {
            if (Input.GetMouseButton(0))
            {
                yaw += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            }


            Quaternion rotation = Quaternion.Euler(0, yaw, 0);
            Vector3 rotatedOffset = rotation * offset;

            Vector3 desiredPos = target.position + rotatedOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
        
    }
}
