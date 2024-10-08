using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class FleeBehavior : MonoBehaviour
{
    public List<Transform> threats; // List of threats to flee from
    private NavMeshAgent agent;

    [Header("Flee Configs")]
    [SerializeField] float fleeDetectionRadius = 10f;
    [SerializeField] float fleeDistance = 5f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 9f;
    [HideInInspector] public static bool isFleeing;

    [Header("Random Movement Configs")]
    [SerializeField] float range = 10f; //radius of sphere

    [Header("Random Movement Configs")]
    [SerializeField] List<AnimatorController> deerAnimations;
    string _currentAnimation = "";

    void Start()
    {
        threats.Add(GameObject.FindGameObjectWithTag("Player").transform);

        agent = GetComponent<NavMeshAgent>();

        ChangeAnimation("Idle", false, .4f);
    }

    void Update()
    {
        isFleeing = IsThreatClose();

        if (isFleeing)
            FleeFromThreats();
        else
            Patrol();

        CheckAnimation();
        Debug.Log($"isFleeing: {isFleeing}");
    }

    private bool IsThreatClose()
    {
        foreach (Transform threat in threats)
        {
            if (threat.CompareTag("Player"))
            {
                if (threat != null && Vector3.Distance(transform.position, threat.position) <= fleeDetectionRadius)
                    return true;
            }
        }

        return false;
    }

    private void FleeFromThreats()
    {
        Vector3 fleeDirection = Vector3.zero; // Initialize to zero

        // Calculate average flee direction from all threats
        foreach (Transform threat in threats)
        {
            if (threat.CompareTag("Player"))
            {
                if (threat != null)
                    fleeDirection += transform.position - threat.position;
            }
        }

        // Normalize the direction to avoid too strong pull
        fleeDirection = fleeDirection.normalized;

        Vector3 fleePosition = transform.position + fleeDirection * 15f;

        NavMesh.SamplePosition(fleePosition, out NavMeshHit navHit, fleeDistance, NavMesh.AllAreas);
        // move speed
        agent.speed = runSpeed;
        agent.SetDestination(navHit.position);

        Debug.Log("Fleeing!");
    }

    void Patrol()
    {
        if (!agent.isActiveAndEnabled) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
            if (RandomPoint(transform.position, range, out Vector3 point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                // agent move speed
                agent.speed = walkSpeed;
                agent.SetDestination(point);
            }
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


    void CheckAnimation()
    {
        if (isFleeing)
        {
            if (agent.velocity.x > 0)
                ChangeAnimation("Run", false);
            else if (agent.velocity.x < 0)
                ChangeAnimation("Run", true);
        }
        else
        {
            if (agent.velocity.x > 0)
                ChangeAnimation("Walk", false);
            else if (agent.velocity.x < 0)
                ChangeAnimation("Walk", true);
        }
    }

    public void ChangeAnimation(string animationName, bool flipSprite = false, float _speed = 1)
    {
        GetComponentInChildren<SpriteRenderer>().flipX = flipSprite;

        if (!_currentAnimation.Contains(animationName))
        {
            foreach (var newAnimation in deerAnimations)
                if (newAnimation.name.Contains(animationName))
                {
                    var animator = GetComponentInChildren<Animator>();

                    _currentAnimation = newAnimation.name;
                    animator.runtimeAnimatorController = newAnimation;

                    if (animator.parameterCount > 0)
                        animator.SetFloat("AnimSpeed", _speed);
                }
        }
    }
}
