using UnityEngine;

public class KeyPuzzle : MonoBehaviour
{
    public Sound keySFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateMachine>().AddKeyToInventory(gameObject.name);
            PlayerConfigs.Instance.AddKeyIcon();
            keySFX.Play(transform.position);
            Destroy(gameObject);
        }
    }
}