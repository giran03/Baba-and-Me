using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject exclamationMark;
    GameObject _player;

    [Header("FOV Configs")]
    [SerializeField] float radius; [Range(0, 360)]
    [SerializeField] float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] bool canSeePlayer;

    [Header("Random Movement Configs")]
    [SerializeField] float range; //radius of sphere

    NavMeshAgent npcAgent;
    float defaultSpeed;
    bool _canAttack;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        npcAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
        defaultSpeed = npcAgent.speed;
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }

    private void Update()
    {
        if (canSeePlayer)
        {
            ChasePlayer();
        }
        else
        {

            Patrol();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                // else
                //     canSeePlayer = false;
            }
            // else
            //     canSeePlayer = false;
        }
        // else if (canSeePlayer)
        //     canSeePlayer = false;
    }

    void Patrol()
    {
        if (!npcAgent.isActiveAndEnabled) return;

        if (npcAgent.remainingDistance <= npcAgent.stoppingDistance)
        {
            npcAgent.speed = defaultSpeed;
            if (RandomPoint(transform.position, range, out Vector3 point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                npcAgent.SetDestination(point);
            }
        }
    }

    void ChasePlayer()
    {
        if (!npcAgent.isActiveAndEnabled) return;

        transform.LookAt(_player.transform.position);
        Vector3 moveTo = Vector3.MoveTowards(transform.position, _player.transform.position, 5f);

        // increase npc move speed
        npcAgent.speed = 7.5f;

        npcAgent.SetDestination(moveTo);
    }

    IEnumerator AttackTiming()
    {
        _canAttack = false;

        yield return new WaitForSeconds(3.0f);

        _canAttack = true;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log($"Im near the player!");
        }
    }

    IEnumerator Attack()
    {
        
        yield return new WaitForSeconds(2f);
        
    }
}
