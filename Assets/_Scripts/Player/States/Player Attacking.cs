using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerAttacking : PlayerBaseState
{
    GameObject _basicAttack;
    RaycastHit hit;

    public PlayerAttacking(PlayerStateMachine currentContext, PlayerStateFactory factory) : base(currentContext, factory)
    {
    }

    public override void CheckSwitchStates()
    {

    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void UpdateState()
    {
        if (PlayerGrab.IsGrabbing) return;

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, CurrentContext.groundLayer);

        if (_basicAttack != null)
        {
            _basicAttack.transform.position = CurrentContext.transform.position;
            _basicAttack.transform.LookAt(hit.point + Vector3.up * 0.4f, Vector3.up);
        }

        if (Input.GetMouseButtonDown(0) && PlayerPrefs.GetString("isPlayerReadyToAttack") == "true")
            MeleeAttack();

        if (!PlayerConfigs.Instance.isMisha) return;
        if (Input.GetMouseButtonDown(1) && PlayerPrefs.GetString("isPlayerReadyToAttack_Ranged") == "true")
            RangedAttack();
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

        AttackStats attackStats = PlayerConfigs.Instance.FindAttackObject("Ranged Attack");
        GameObject arrow = Object.Instantiate(attackStats.prefab, CurrentContext.transform.position + Vector3.up * .8f, Quaternion.identity);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();

        Vector3 direction = (hit.point + Vector3.up * .5f - CurrentContext.transform.position).normalized;

        if (direction.x < 0)
        {
            arrow.GetComponentInChildren<SpriteRenderer>().flipX = true;
            arrow.GetComponentInChildren<SpriteRenderer>().flipY = true;
        }
        else
        {
            arrow.GetComponentInChildren<SpriteRenderer>().flipX = false;
            arrow.GetComponentInChildren<SpriteRenderer>().flipY = false;
        }

        arrow.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        arrowRb.AddForce(10f * attackStats.arrowForce * direction, ForceMode.Impulse);

        CurrentContext.StartCoroutine(RangedAttackCooldown(attackStats.AttackSpeed));
        CurrentContext.StartCoroutine(DestroyAttackObject(arrow, attackStats.AttackDestroyTime));

        HUDHandler.Instance.StartIconCooldown("Ranged", attackStats.AttackSpeed);
    }

    IEnumerator MeleeAttackCooldown(float duration)
    {
        Debug.Log($"RELOADING ATTACK FOR {duration}!");
        yield return new WaitForSeconds(duration);
        PlayerPrefs.SetString("isPlayerReadyToAttack", "true");
    }

    IEnumerator RangedAttackCooldown(float duration)
    {
        Debug.Log($"RELOADING ATTACK FOR {duration}!");
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
        Debug.Log($"Disabled attack collider!");
    }
}