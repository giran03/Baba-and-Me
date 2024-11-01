using System.Collections;
using UnityEngine;

public class DoorKeyPuzzle : MonoBehaviour
{
    [SerializeField] GameObject keyObj;
    [SerializeField] Sound portalSFX;

    MovePlayer _movePlayer = new();
    PlayerStateMachine _player;

    string _requiredKey;

    bool _canInteractDoor = false;

    private void Start()
    {
        _requiredKey = keyObj.name;
        _player = FindAnyObjectByType<PlayerStateMachine>();
    }



    private void OnTriggerEnter(Collider other) => _canInteractDoor = other.CompareTag("Player");

    private void OnTriggerStay(Collider other)
    {
        if (_player.HasKey(_requiredKey) && _canInteractDoor)
        {
            _canInteractDoor = false;
            //sfx
            portalSFX.Play(transform.position);

            _player.KeysInventory.Remove(_requiredKey);
            PlayerConfigs.Instance.RemoveKeyIcon();
            StartCoroutine(_movePlayer.MoveThisPlayer(gameObject, other, 1, DestroyItems()));
            _player.ClearKeyInventory();
        }
    }

    private void OnTriggerExit(Collider other) => _canInteractDoor = false;

    IEnumerator DestroyItems()
    {
        yield return new WaitForSeconds(2);
        PlayerStateMachine.ResumePlayer();
        Destroy(gameObject);
    }
}