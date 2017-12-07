using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = -50.0f;
    private const float Y_ANGLE_MAX = 5.0f;

    Vector3 offset = new Vector3(0, 1, 0);
    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 3.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensivityX = 4f;
    private float sensivityY = 1.0f;

    Vector3 position;

    public LayerMask mask;
    private void start()
    {
        position = Vector3.zero;
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensivityX;
        currentY += Input.GetAxis("Mouse Y") * sensivityY;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();

        position = offset + lookAt.position + rotation * dir;

        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast( lookAt.transform.position, position, out wallHit,mask))
        {

            Debug.Log(wallHit.transform.name);
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            position = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, position.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }

        #endregion
        camTransform.position = position;

        camTransform.LookAt(lookAt.position + offset);
    }

}
