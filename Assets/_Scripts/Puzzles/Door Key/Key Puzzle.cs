using UnityEngine;

public class KeyPuzzle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateMachine>().AddKeyToInventory(gameObject.name);
            Destroy(gameObject);
        }
    }
}
