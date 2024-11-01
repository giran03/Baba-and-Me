using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnEnable : MonoBehaviour
{
    public Sound sfx;
    private void OnEnable() => sfx.Play(transform.position);
}
