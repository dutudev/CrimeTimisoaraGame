using Unity.Mathematics;
using UnityEngine;

public class TriangleBillboard : MonoBehaviour
{
  
    void Update()
    {
        transform.localRotation = Quaternion.LookRotation((transform.position - CameraMovement.instance.GetCameraPos()).normalized);
    }
}
