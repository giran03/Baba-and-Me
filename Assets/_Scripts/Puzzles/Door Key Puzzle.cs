using UnityEngine;

public class DoorKeyPuzzle : MonoBehaviour
{
    public GameObject keyObj;

    string _requiredKey;

    private void Start()
    {
        _requiredKey = keyObj.name;
        keyObj.GetComponent<KeyPuzzle>().PuzzleManager = this;
        Debug.Log($"PuzzleManager: {keyObj.GetComponent<KeyPuzzle>().PuzzleManager}");
    }

    void UnlockDoor(string currentKey)
    {
        Debug.Log($"Current key: {currentKey} Required key: {_requiredKey}");
        Debug.Log($"{_requiredKey == currentKey}");
        if (_requiredKey == currentKey)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string playerKeyFromInventory = other.GetComponent<PlayerStateMachine>().GetKeyFromInventory(_requiredKey);
            UnlockDoor(playerKeyFromInventory);
        }
    }
}
