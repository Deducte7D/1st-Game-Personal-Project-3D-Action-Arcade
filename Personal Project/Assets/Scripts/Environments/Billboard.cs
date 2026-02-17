using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    public float smoothSpeed = 10f;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        transform.forward = cam.transform.forward;
        //Vector3 targetDir = cam.transform.forward;
        //transform.forward = Vector3.Lerp(transform.forward, targetDir, Time.deltaTime * smoothSpeed);
    }

}
