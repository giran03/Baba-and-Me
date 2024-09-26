using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : EnemyBaseState
{
    Vector3 chasePoint;
    GameObject spawnedHitBox;

    public EnemyAttack(EnemyStateMachine currentContext, EnemyStateFactory factory) : base(currentContext, factory)
    {
        IsRootState = true;
        InitializeSubState();
    }


    public override void EnterState()
    {
        CurrentContext.StartCoroutine(AttackPlayer());

        IEnumerator AttackPlayer()
        {
            while (true)
            {
                if (PlayerPrefs.GetString($"{CurrentContext.name}_isEnemyReadyToAttack") == "false")
                    yield return new WaitUntil(() => PlayerPrefs.GetString($"{CurrentContext.name}_isEnemyReadyToAttack") == "true");

                CurrentContext.StartCoroutine(SpawnHitbox());
                CurrentContext.StartCoroutine(AttackCooldown(CurrentContext.attackRate));
            }
        }
    }

    public override void UpdateState()
    {
        MoveEnemy();
        CheckAnimation();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchStates()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void OnDisable()
    {
        // PlayerPrefs.DeleteKey($"{CurrentContext.name}_isEnemyReadyToAttack");
        // Debug.Log($"Enemy prefs key deleted: {CurrentContext.name}");
        if (spawnedHitBox != null)
            Object.Destroy(spawnedHitBox);
    }

    private void MoveEnemy()
    {
        var _player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(CurrentContext.transform.position, _player.transform.position);


        var lookPos = _player.transform.position - CurrentContext.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        CurrentContext.transform.rotation = Quaternion.Slerp(CurrentContext.transform.rotation, rotation, Time.deltaTime * .7f);

        if (distance >= 6f)
            CurrentContext._navMeshAgent.SetDestination(_player.transform.position);
        else if (CurrentContext._navMeshAgent.remainingDistance <= .8f)
        {
            GetRandomPointAroundPlayer(_player.transform);
            CurrentContext._navMeshAgent.SetDestination(chasePoint);
        }
        else
            GetRandomPointAroundPlayer(_player.transform);
    }

    IEnumerator AttackCooldown(float duration)
    {
        PlayerPrefs.SetString($"{CurrentContext.name}_isEnemyReadyToAttack", "false");

        yield return new WaitForSeconds(duration);

        PlayerPrefs.SetString($"{CurrentContext.name}_isEnemyReadyToAttack", "true");
    }

    IEnumerator SpawnHitbox()
    {
        if (CurrentContext._navMeshAgent.velocity.x > 0)
            ChangeAnimation("attack");
        else if (CurrentContext._navMeshAgent.velocity.x < 0)
            ChangeAnimation("attack", true);

        var _player = GameObject.FindWithTag("Player");
        var lookPos = _player.transform.position - CurrentContext.transform.position;

        spawnedHitBox = Object.Instantiate(CurrentContext.attackPrefab, CurrentContext.transform.position + Vector3.forward * .5f, Quaternion.LookRotation(lookPos + Vector3.up * .3f, Vector3.up));
        yield return new WaitForSeconds(.4f);

        ChangeAnimation("run");
        Object.Destroy(spawnedHitBox);
    }

    void GetRandomPointAroundPlayer(Transform _player)
    {
        float distance = Vector3.Distance(CurrentContext.transform.position, _player.position);

        var lookPos = _player.position - CurrentContext.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        CurrentContext.transform.rotation = Quaternion.Slerp(CurrentContext.transform.rotation, rotation, Time.deltaTime * .7f);

        if (distance > 7f)
            CurrentContext._navMeshAgent.SetDestination(_player.position);
        else
        {
            Vector3 direction = (CurrentContext.transform.position - _player.position).normalized;
            Vector3 point = _player.position + (Quaternion.Euler(0, Random.Range(-180f, 180f), 0) * direction * Random.Range(1f, 10f));

            if (NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                chasePoint = hit.position;
            else
                chasePoint = _player.position + (Quaternion.Euler(0, Random.Range(-180f, 180f), 0) * direction * 5f);
        }
    }
}