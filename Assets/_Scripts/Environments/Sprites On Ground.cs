using UnityEngine;

public class SpritesOnGround : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    private void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }
    }
}