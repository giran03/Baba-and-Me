using System.Collections;
using UnityEngine;

public class PlayerWalking : PlayerBaseState
{
    float _dashCooldown;
    private bool isFootstepsOnCd;

    public PlayerWalking(PlayerStateMachine context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log($"ℹ️ Entered Walk State");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        CheckAnimation();

        //sfx
        if (!isFootstepsOnCd)
            CurrentContext.StartCoroutine(PlayFootsteps());

        if (PlayerGrab.IsGrabbing) return;

        if (Input.GetKeyDown(KeyCode.Space) && _dashCooldown <= 0)
        {
            _dashCooldown = 0.5f;
            CurrentContext.StartCoroutine(Dash());
            HUDHandler.Instance.StartIconCooldown("Dash", _dashCooldown);
            Debug.Log($"DASHING~!");
        }

        _dashCooldown -= Time.deltaTime;
    }

    IEnumerator PlayFootsteps()
    {
        isFootstepsOnCd = true;
        PlayerConfigs.Instance.gravelFootstepsSFX.PlayWithRandomPitch(CurrentContext.transform.position);
        yield return new WaitForSeconds(.5f);
        isFootstepsOnCd = false;
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            SwitchState(Factory.Idle());
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Attacking());
    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    IEnumerator Dash()
    {
        float timer = 0.2f;
        Vector3 direction = Vector3.zero;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        //sfx
        PlayerConfigs.Instance.dashSFX[Random.Range(0, PlayerConfigs.Instance.dashSFX.Length)].Play(CurrentContext.transform.position);

        if (x != 0 || y != 0)
        {
            direction = new Vector3(x, 0, y).normalized;
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            CurrentContext.gameObject.GetComponent<Rigidbody>().velocity = direction * PlayerConfigs.Instance.dashSpeed;
            CurrentContext.StartCoroutine(DisableColliderForSeconds(PlayerConfigs.Instance.invincibilityDuration));
            yield return null;
        }
        CurrentContext.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    IEnumerator DisableColliderForSeconds(float seconds)
    {
        CurrentContext.transform.GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer);
        spriteRenderer.material.color = Color.grey;

        CurrentContext.gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(seconds);
        CurrentContext.gameObject.GetComponent<Collider>().enabled = true;
        spriteRenderer.material.color = Color.white;
    }
}