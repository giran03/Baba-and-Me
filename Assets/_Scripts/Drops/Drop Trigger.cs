using System.Collections;
using UnityEngine;

public class DropTrigger : MonoBehaviour
{
    // CONFIGS
    float healAmount = 2;
    int scoreIncreaseAmount = 5;
    int powerIncreaseAmount = 5;
    bool isDetected;
    bool canPickup = false;
    GameObject player;

    private void OnEnable()
    {
        StartCoroutine(CanPickup());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
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
    }

    private void OnTriggerStay(Collider other) => PickUp(other);

    void PickUp(Collider other)
    {
        if (other.CompareTag("Player") && canPickup)
        {
            switch (gameObject.tag)
            {
                case "Health Drop":
                    PlayerConfigs.Instance.Heal(other, healAmount);
                    Debug.Log("HP DROP COLLECTED!");
                    break;

                case "XP Drop":
                    PlayerConfigs.Instance.IncreaseScore(scoreIncreaseAmount);
                    Debug.Log("XP DROP COLLECTED!");
                    break;

                case "Power Drop":
                    PlayerConfigs.Instance.PowerIncrease(other, powerIncreaseAmount);
                    Debug.Log("POWER DROP COLLECTED!");
                    break;
            }

            Destroy(gameObject);
            isDetected = false;
        }
    }
}
