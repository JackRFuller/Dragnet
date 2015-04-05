using UnityEngine;
using System.Collections;

public class NpcClass : MonoBehaviour {

    public float speed;
    public float range;
    public float minAttackRange;
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

        if (Physics.Raycast(transform.position, rayDir * range, out hit, LineOfSightMask))
        {
            if (hit.collider.gameObject.tag == "Player")
                return true;
            else
                return false;
        }

        return false;
    }

    public virtual void Attack()
    {

    }

    public void Patrol()
    {
       
    }
}
