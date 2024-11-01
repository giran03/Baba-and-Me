using UnityEngine;
using UnityEngine.Rendering;

public class SpritesLookAt : MonoBehaviour
{
    public bool DoUpdate { get; set; }
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out var renderer))
            _spriteRenderer = renderer;
        else
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _spriteRenderer.shadowCastingMode = ShadowCastingMode.On;
            _spriteRenderer.receiveShadows = true;
        }
    }

    private void FixedUpdate()
    {
        if (!DoUpdate)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.shadowCastingMode = ShadowCastingMode.Off;
            return;
        }
        else
            _spriteRenderer.shadowCastingMode = ShadowCastingMode.On;

        Vector3 cameraPosition = Camera.main.transform.position;

        cameraPosition.y = transform.position.y;
        transform.LookAt(cameraPosition, Vector3.up);
        transform.Rotate(transform.rotation.x, 180, transform.rotation.z);
    }
}