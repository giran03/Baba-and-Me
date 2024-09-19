using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesLookAt : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        cameraPosition.y = transform.position.y;
        transform.LookAt(cameraPosition);
        transform.Rotate(0, 180, 0);
    }
}
