using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrigger : MonoBehaviour
{
    bool isDetected;
    bool canPickup = false;
    GameObject player;

    private void OnEnable()
    {
        StartCoroutine(CanPickup());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other) => PickUp(other);

    void PickUp(Collider other)
    {
        if (other.CompareTag("Player") && canPickup)
        {
            Destroy(gameObject);
            isDetected = false;
            Debug.Log("DROP COLLECTED!");
        }
    }

    private void Update()
    {
        if (!canPickup) return;

        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer <= PlayerConfigs.Instance.dropDetectionRadius)
        {
            isDetected = true;

            foreach (var item in GetComponents<Collider>())
                item.material = null;
        }

        if (isDetected)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(direction * 15f, ForceMode.Acceleration);
        }
    }

    IEnumerator CanPickup()
    {
        yield return new WaitForSeconds(1.25f);
        canPickup = true;
        Debug.Log($"PICKUPPP!");
    }
}
