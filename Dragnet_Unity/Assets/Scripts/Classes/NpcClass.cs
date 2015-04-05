using UnityEngine;
using System.Collections;

public class NpcClass : MonoBehaviour {

    public float speed;
    public float range;
    public float loseRange;
    public int health;
    public GameObject Player;
    public LayerMask LineOfSightMask;
    public Transform[] patrolPoints;
    [HideInInspector]
    public NavMeshAgent navMesh;
    [HideInInspector]
    private int currWaypoint;

    public bool CanSeePlayer(Vector3 rayStart, GameObject target)
    {
        RaycastHit hit;
        Vector3 rayDir = target.transform.position - transform.position;

        Debug.DrawRay(transform.position, rayDir, Color.red);

        if (Physics.Raycast(transform.position, rayDir, out hit))
        {
             if (hit.collider.gameObject.tag == "Player")
                return true;
        }
        return false;
    }

    public virtual void Attack()
    {

    }

    public void Patrol()
    {
        if (currWaypoint < patrolPoints.Length)
        {
            Vector3 targetPos = patrolPoints[currWaypoint].position;
            targetPos.y = transform.position.y;
            navMesh.SetDestination(targetPos);

            if (Vector3.Distance(transform.position, targetPos) < 0.5)
            {
                currWaypoint++;
            }
        }
        else
        {
            currWaypoint = 0;
        }
    }
}
