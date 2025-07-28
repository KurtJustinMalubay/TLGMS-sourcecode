using UnityEngine;
using static playerScript;

public class cameraScript : MonoBehaviour
{
    public Camera cameraP;
    public Transform Camera;
    public EffectsScript effectsScript;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.5f;
    private float max = 7f;
    private float min = 4f;
    private float def = 5.61f;

    void FixedUpdate()
    {
        Vector3 playerPos = PlayerTracker.playerTransform.position + Vector3.back * 10f;
        playerPos.z = -2f;
        if (!PlayerTracker.isSprinting)
            Camera.position = Vector3.SmoothDamp(Camera.position, playerPos, ref velocity, smoothTime);
        else
            Camera.position = Vector3.SmoothDamp(Camera.position, playerPos, ref velocity, 0.3f);
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll < 0)
            {
                cameraP.orthographicSize += Time.deltaTime * 10;
                if (cameraP.orthographicSize >= max)
                    cameraP.orthographicSize = max;
            }
            else if (scroll > 0)
            {
                cameraP.orthographicSize -= Time.deltaTime * 10;
                if (cameraP.orthographicSize <= min)
                    cameraP.orthographicSize = min;
            }
        }
        if (Input.GetKey(KeyCode.Mouse2))
            cameraP.orthographicSize = def;
    }
}
