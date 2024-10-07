using UnityEngine;

public class DoorKeyPuzzle : MonoBehaviour
{
    [SerializeField] GameObject keyObj;

    PlayerStateMachine _player;

    string _requiredKey;

    bool _canInteractDoor;

    private void Start()
    {
        _requiredKey = keyObj.name;
        keyObj.GetComponent<KeyPuzzle>().PuzzleManager = this;
        _player = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }


    private void Update()
    {
        if (_player.HasKey(_requiredKey) && _canInteractDoor)
        {
            _canInteractDoor = false;
            _player.KeysInventory.Remove(_requiredKey);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _canInteractDoor = other.CompareTag("Player");
    }

    private void OnTriggerExit(Collider other)
    {
        _canInteractDoor = false;
    }
}