using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    //LateUpdate is called after all Update functions have been called.
    private void LateUpdate()
    {
        Camera cam = PerspectiveController.ActiveCamera;

        if (cam == null)
        {
            return;
        }

        transform.LookAt(cam.transform.position);
    }
}
