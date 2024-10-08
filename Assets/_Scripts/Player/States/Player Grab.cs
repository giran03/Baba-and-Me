using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public static bool IsGrabbing {get; set;}
    float defaultRbMass;
    Rigidbody boulderRb;
    FixedJoint fixedJoint;
    Outline boulderOutline;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsGrabbing)
        {
            UpdateGrab();
        }
        else if (Input.GetKeyUp(KeyCode.E) && IsGrabbing)
        {
            UpdateRelease();
            StartCoroutine(GrabbingCooldown());
        }
    }

    IEnumerator GrabbingCooldown()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log($"Grab done CD");
        IsGrabbing = false;
    }

    void UpdateGrab()
    {
        Collider closestHitCollider = null;
        float closestDistanceSqr = Mathf.Infinity;
        var hitColliders = new Collider[10];
        var numHits = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, hitColliders);
        for (int i = 0; i < numHits; i++)
        {
            var hitCollider = hitColliders[i];
            if (hitCollider.gameObject.CompareTag("Grabbable"))
            {
                var distanceSqr = (hitCollider.transform.position - transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestHitCollider = hitCollider;
                    closestDistanceSqr = distanceSqr;
                }
            }
        }

        if (closestHitCollider != null)
        {
            IsGrabbing = true;

            fixedJoint = gameObject.AddComponent<FixedJoint>();
            boulderRb = closestHitCollider.transform.GetComponent<Rigidbody>();
            boulderOutline = closestHitCollider.transform.GetComponent<Outline>();
            boulderOutline.enabled = true;

            closestHitCollider.transform.position = closestHitCollider.transform.position + Vector3.up * .8f;

            defaultRbMass = boulderRb.mass;
            boulderRb.mass = 4f;

            fixedJoint.connectedBody = boulderRb;
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