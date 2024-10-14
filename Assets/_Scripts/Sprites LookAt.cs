using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesLookAt : MonoBehaviour
{
    bool DoUpdate;

    private void OnBecameVisible()
    {
        DoUpdate = true;
    }

    private void OnBecameInvisible()
    {
        DoUpdate = false;
    }

    private void LateUpdate()
    {
        if (!DoUpdate) return;
        
        Vector3 cameraPosition = Camera.main.transform.position;

        cameraPosition.y = transform.position.y;
        transform.LookAt(cameraPosition, Vector3.up);
        transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
    }

}
