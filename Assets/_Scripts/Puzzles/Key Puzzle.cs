using UnityEngine;

public class KeyPuzzle : MonoBehaviour
{
    public DoorKeyPuzzle PuzzleManager { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateMachine>().AddKeyToInventory(name);
            Destroy(gameObject);
        }
    }
}
