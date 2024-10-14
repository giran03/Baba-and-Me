using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class FleeBehavior : MonoBehaviour
{
    GameObject _player;
    private NavMeshAgent agent;

    [Header("Flee Configs")]
    [SerializeField] float fleeDetectionRadius = 10f;
    [SerializeField] float fleeDistance = 5f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 9f;

    [Header("Random Movement Configs")]
    [SerializeField] float range = 10f; //radius of sphere

    [Header("Random Movement Configs")]
    [SerializeField] List<AnimatorController> deerAnimations;
    string _currentAnimation = "";

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();

        ChangeAnimation("Idle", false, .4f);
    }

    void Update()
    {
        if (IsThreatClose())
            FleeFromThreats();
        else
            Patrol();

        CheckAnimation();
    }

    private bool IsThreatClose()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= fleeDetectionRadius)
            return true;

        return false;
    }

    // TODO: IMPROVE THIS!!! | Destroy after flee; Respawn; if hit by an arrow;
    private void FleeFromThreats()
    {
        //get the closest threat
        Transform closestThreat = null;
        float closestDistance = float.MaxValue;
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < closestDistance)
        {
            closestThreat = _player.transform;
            closestDistance = distance;
        }

        //flee from the closest threat
        Vector3 fleeDirection = (transform.position - closestThreat.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;

        // agent move speed
        agent.speed = runSpeed;
        agent.SetDestination(fleePosition);
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
        if (IsThreatClose())
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
