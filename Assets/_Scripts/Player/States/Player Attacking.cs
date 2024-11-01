using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttacking : PlayerBaseState
{
    GameObject _basicAttack;
    GameObject _spawnedUltimate;
    RaycastHit hit;

    public PlayerAttacking(PlayerStateMachine currentContext, PlayerStateFactory factory) : base(currentContext, factory) { }

    public override void CheckSwitchStates() { }
    public override void EnterState() { }
    public override void ExitState() { }
    public override void InitializeSubState() { }
    public override void OnTriggerEnter(Collider other) { }

    public override void UpdateState()
    {
        // follow player
        if (_spawnedUltimate != null)
            _spawnedUltimate.transform.position = Vector3.Slerp(_spawnedUltimate.transform.position, CurrentContext.transform.position, Time.deltaTime * 5f);

        if (PlayerGrab.IsGrabbing) return;

        if (!EventSystem.current.IsPointerOverGameObject())
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, CurrentContext.groundLayer);

        if (_basicAttack != null)
        {
            _basicAttack.transform.position = CurrentContext.transform.position;
            _basicAttack.transform.LookAt(hit.point + Vector3.up * 0.4f, Vector3.up);
        }

        if (Input.GetMouseButton(0) && PlayerPrefs.GetString("isPlayerReadyToAttack") == "true")
            MeleeAttack();

        if (PlayerConfigs.Instance.isMisha)
        {
            if (Input.GetMouseButton(1) && PlayerPrefs.GetString("isPlayerReadyToAttack_Ranged") == "true")
                RangedAttack();
        }

        if (CurrentContext.canUseUltimate)
        {
            GameObject _ultimateAttack;
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerPrefs.SetString("isPlayerReadyToAttack_Ranged", "true");
                PlayerPrefs.SetString("isPlayerReadyToAttack", "true");

                CurrentContext.canUseUltimate = false;
                CurrentContext.playerPower = 0;
                CurrentContext.UpdatePowerBar();

                if (PlayerConfigs.Instance.isMisha)
                    _ultimateAttack = PlayerConfigs.Instance.ultimateAttackPrefab[0];
                else
                    _ultimateAttack = PlayerConfigs.Instance.ultimateAttackPrefab[1];

                _spawnedUltimate = Object.Instantiate(_ultimateAttack, CurrentContext.transform.position, Quaternion.identity);

                CurrentContext.StartCoroutine(DestroyAfterDelay(4f, _spawnedUltimate));
            }
        }
    }

    IEnumerator DestroyAfterDelay(float delay, GameObject obectToDestroy)
    {
        yield return new WaitForSeconds(delay);
        Object.Destroy(obectToDestroy);
    }

    void MeleeAttack()
    {
        // seemless countdown for attack through different scripts;
        PlayerPrefs.SetString("isPlayerReadyToAttack", "false");

        AttackStats attackStats = PlayerConfigs.Instance.FindAttackObject("Basic Attack");
        _basicAttack = Object.Instantiate(attackStats.prefab, CurrentContext.transform.position, Quaternion.identity);

        CurrentContext.StartCoroutine(MeleeAttackCooldown(attackStats.AttackSpeed));

        _basicAttack.transform.Rotate(Vector3.right, -90f);

        if (hit.point.x < CurrentContext.transform.position.x)
            _basicAttack.GetComponentInChildren<SpriteRenderer>().flipX = true;
        else
            _basicAttack.GetComponentInChildren<SpriteRenderer>().flipX = false;

        CurrentContext.StartCoroutine(DestroyAttackObject(_basicAttack, attackStats.AttackDestroyTime));
        CurrentContext.StartCoroutine(DisableColliderAfterTime(0.1f));

        HUDHandler.Instance.StartIconCooldown("Melee", attackStats.AttackSpeed);
    }


    void RangedAttack()
    {
        // seemless countdown for attack through different scripts;
        PlayerPrefs.SetString("isPlayerReadyToAttack_Ranged", "false");

        //sfx
        PlayerConfigs.Instance.bowSFX[Random.Range(0, PlayerConfigs.Instance.bowSFX.Length)].Play(CurrentContext.transform.position);

        Vector3 direction = (hit.point + Vector3.up * .5f - CurrentContext.transform.position).normalized;
        AttackStats attackStats = PlayerConfigs.Instance.FindAttackObject("Ranged Attack");
        GameObject arrow = Object.Instantiate(attackStats.prefab, CurrentContext.transform.position + Vector3.up * .8f, Quaternion.LookRotation(hit.point - CurrentContext.transform.position, Vector3.up));
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();

        arrowRb.AddForce(10f * attackStats.arrowForce * direction, ForceMode.Impulse);

        CurrentContext.StartCoroutine(RangedAttackCooldown(attackStats.AttackSpeed));
        CurrentContext.StartCoroutine(DestroyAttackObject(arrow, attackStats.AttackDestroyTime));

        HUDHandler.Instance.StartIconCooldown("Ranged", attackStats.AttackSpeed);
    }

    IEnumerator MeleeAttackCooldown(float duration)
    {
        // Debug.Log($"RELOADING ATTACK FOR {duration}!");
        yield return new WaitForSeconds(duration);
        PlayerPrefs.SetString("isPlayerReadyToAttack", "true");
    }

    IEnumerator RangedAttackCooldown(float duration)
    {
        // Debug.Log($"RELOADING ATTACK FOR {duration}!");
        yield return new WaitForSeconds(duration);
        PlayerPrefs.SetString("isPlayerReadyToAttack_Ranged", "true");
    }

    IEnumerator DestroyAttackObject(GameObject attackObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        Object.Destroy(attackObject);
    }

    IEnumerator DisableColliderAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _basicAttack.GetComponent<Collider>().enabled = false;
    }
}