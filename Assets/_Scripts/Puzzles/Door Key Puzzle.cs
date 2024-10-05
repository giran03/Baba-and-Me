using UnityEngine;

public class DoorKeyPuzzle : MonoBehaviour
{
    public GameObject keyObj;

    PlayerStateMachine _player;

    string _requiredKey;

    bool CanInteractDoor { get; set; }

    string playerKeyFromPlayerInventory;

    private void Start()
    {
        _requiredKey = keyObj.name;
        keyObj.GetComponent<KeyPuzzle>().PuzzleManager = this;
        _player = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (!CanInteractDoor) return;

        if (_requiredKey == playerKeyFromPlayerInventory)
        {
            _player.KeysInventory.Remove(_requiredKey);
            Destroy(gameObject);

            Debug.Log($"Unlocked puzzle!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteractDoor = true;
            playerKeyFromPlayerInventory = _player.GetKeyFromInventory(_requiredKey);
        }
        else
            CanInteractDoor = false;
    }
}