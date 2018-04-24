using UnityEngine;

public class FaceCamera_Late : MonoBehaviour
{
    public Camera cameraToLookAt;

    void Start()
    {
        //transform.Rotate( 180,0,0 );
    }

    void Update()
    {
        //Vector3 v = cameraToLookAt.transform.position - transform.position;
        //v.x = v.z = 0.0f;
        //transform.LookAt(cameraToLookAt.transform.position - v);
        //transform.Rotate(0, 180, 0);

        var fwd = Camera.main.transform.forward;
        fwd.y = 0.0F;
        transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.forward,
            cameraToLookAt.transform.rotation * Vector3.up);
    }
}
