using UnityEngine;

public class DoorKeyPuzzle : MonoBehaviour
{
    [SerializeField] GameObject keyObj;

    PlayerStateMachine _player;

    string _requiredKey;

    bool _canInteractDoor = false;

    private void Start()
    {
        _requiredKey = keyObj.name;
        _player = FindAnyObjectByType<PlayerStateMachine>();
        Debug.Log($"_player: {_player}");
    }


    private void Update()
    {
        if (_player.HasKey(_requiredKey) && _canInteractDoor)
        {
            _canInteractDoor = false;
            _player.KeysInventory.Remove(_requiredKey);
            Destroy(gameObject);

            _player.ClearKeyInventory();
        }
    }

    private void OnTriggerEnter(Collider other) => _canInteractDoor = other.CompareTag("Player");

    private void OnTriggerExit(Collider other) => _canInteractDoor = false;
}