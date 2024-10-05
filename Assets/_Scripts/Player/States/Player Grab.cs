using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    bool isGrabbing = false;
    float defaultRbMass;
    Rigidbody boulderRb;
    FixedJoint fixedJoint;
    Outline boulderOutline;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isGrabbing)
        {
            UpdateGrab();
        }
        else if (Input.GetKeyUp(KeyCode.E) && !isGrabbing)
        {
            UpdateRelease();
            StartCoroutine(GrabbingCooldown());
        }
    }

    IEnumerator GrabbingCooldown()
    {
        isGrabbing = true;
        Debug.Log($"Starting CD");

        yield return new WaitForSeconds(1f);

        Debug.Log($"Grab done CD");
        isGrabbing = false;
    }

    void UpdateGrab()
    {
        var hitColliders = new Collider[10];
        var numHits = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, hitColliders);
        for (int i = 0; i < numHits; i++)
        {
            var hitCollider = hitColliders[i];
            {
                if (hitCollider.gameObject.CompareTag("Grabbable"))
                {
                    fixedJoint = gameObject.AddComponent<FixedJoint>();
                    boulderRb = hitCollider.transform.GetComponent<Rigidbody>();
                    boulderOutline = hitCollider.transform.GetComponent<Outline>();
                    boulderOutline.enabled = true;

                    hitCollider.transform.position = hitCollider.transform.position + Vector3.up * .8f;

                    defaultRbMass = boulderRb.mass;
                    boulderRb.mass = 4f;

                    fixedJoint.connectedBody = boulderRb;
                }
            }
        }
    }

    void UpdateRelease()
    {
        if (fixedJoint != null)
        {
            boulderRb.mass = defaultRbMass;
            boulderOutline.enabled = false;

            Destroy(fixedJoint);
        }
    }
}