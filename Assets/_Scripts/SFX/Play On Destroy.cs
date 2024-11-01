using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnDestroy : MonoBehaviour
{
    public Sound sfx;
    Transform spawnTransform;
    private void Start() => spawnTransform = transform;
    private void OnDestroy() => sfx.Play(spawnTransform.position);
}
