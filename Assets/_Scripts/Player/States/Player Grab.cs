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
        var closestObject = CheckClosestObject();

        if (closestObject != null)
        {
            boulderOutline = closestObject.GetComponent<Outline>();

            if (!IsGrabbing)
            {
                if (boulderOutline != null)
                {
                    boulderOutline.OutlineColor = Color.cyan;
                    boulderOutline.OutlineWidth = 5f;
                    boulderOutline.enabled = true;
                }
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
        {
            UpdateGrab();
        }
        // else
        // {
        //     UpdateRelease();
        //     StartCoroutine(GrabbingCooldown());
        // }

        if (Input.GetKeyUp(KeyCode.E))
        {
            UpdateRelease();
            StartCoroutine(GrabbingCooldown());
        }
    }

    IEnumerator GrabbingCooldown()
    {
        isGrabOnCooldown = true;
        HUDHandler.Instance.StartIconCooldown("Grab", 1f);

        yield return new WaitForSeconds(1f);

        Debug.Log($"Grab done CD");
        isGrabOnCooldown = false;
        IsGrabbing = false;
    }

    void UpdateGrab()
    {
        if (CheckClosestObject() == null) return;
        IsGrabbing = true;

        //sfx
        PlayerConfigs.Instance.boulderSFX[0].Play(transform.position);

        var boulder = CheckClosestObject();
        fixedJoint = gameObject.AddComponent<FixedJoint>();

        boulderRb = boulder.transform.GetComponent<Rigidbody>();
        if (boulderRb != null && boulderOutline != null)
        {
            boulderOutline.OutlineColor = Color.green;
            boulderOutline.OutlineWidth = 2f;
            boulderOutline.enabled = true;

            boulder.transform.position = boulder.transform.position + Vector3.up * .8f;

            defaultRbMass = boulderRb.mass;
            boulderRb.mass = 4f;

            fixedJoint.connectedBody = boulderRb;
        }
    }

    void UpdateRelease()
    {
        if (fixedJoint != null)
        {
            if (boulderRb != null)
            {
                boulderRb.mass = defaultRbMass;
                boulderRb = null;
            }

            if (boulderOutline != null)
            {
                boulderOutline.OutlineColor = Color.cyan;
                boulderOutline.OutlineWidth = 5f;
                boulderOutline.enabled = false;
            }

            //sfx
            PlayerConfigs.Instance.boulderSFX[1].Play(transform.position);
            Destroy(fixedJoint);
        }
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
}