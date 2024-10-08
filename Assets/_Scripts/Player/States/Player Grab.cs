using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public static bool IsGrabbing { get; set; }
    float defaultRbMass;
    Rigidbody boulderRb;
    FixedJoint fixedJoint;
    Outline boulderOutline;
    bool isGrabOnCooldown;

    void Update()
    {
        if (CheckClosestObject() != null)
        {
            boulderOutline = CheckClosestObject().GetComponent<Outline>();

            if (!IsGrabbing)
            {
                boulderOutline.OutlineColor = Color.cyan;
                boulderOutline.OutlineWidth = 5f;
                boulderOutline.enabled = true;
            }
        }
        else
        {
            if (boulderOutline != null)
            {
                boulderOutline.enabled = false;
                boulderOutline = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !IsGrabbing && !isGrabOnCooldown)
            UpdateGrab();
            
        if (Input.GetKeyUp(KeyCode.E) && IsGrabbing && !isGrabOnCooldown)
            StartCoroutine(GrabbingCooldown());
    }

    IEnumerator GrabbingCooldown()
    {
        isGrabOnCooldown = true;
        UpdateRelease();
        
        yield return new WaitForSeconds(1f);

        Debug.Log($"Grab done CD");
        isGrabOnCooldown = false;
        IsGrabbing = false;
    }

    public GameObject CheckClosestObject()
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
            return closestHitCollider.gameObject;
        }
        return null;
    }

    void UpdateGrab()
    {
        IsGrabbing = true;

        var boulder = CheckClosestObject();
        fixedJoint = gameObject.AddComponent<FixedJoint>();

        boulderRb = boulder.transform.GetComponent<Rigidbody>();
        boulderOutline.OutlineColor = Color.green;
        boulderOutline.OutlineWidth = 2f;
        boulderOutline.enabled = true;

        boulder.transform.position = boulder.transform.position + Vector3.up * .8f;

        defaultRbMass = boulderRb.mass;
        boulderRb.mass = 4f;

        fixedJoint.connectedBody = boulderRb;
    }

    void UpdateRelease()
    {
        if (fixedJoint != null)
        {
            boulderRb.mass = defaultRbMass;
            boulderRb = null;

            boulderOutline.OutlineColor = Color.cyan;
            boulderOutline.OutlineWidth = 5f;
            boulderOutline.enabled = false;

            Destroy(fixedJoint);
        }
    }
}