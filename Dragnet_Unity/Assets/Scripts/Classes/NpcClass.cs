using UnityEngine;
using System.Collections;

public class NpcClass : MonoBehaviour
{
    public enum Behaviours
    {
        Idle,
        Aggressive,
        Enraged
    }
    public Behaviours npcBehaviours;

    [Header("How Far The NPC Can See")]
    public float sightRange;
    [Header("When the NPC will lose interested once in chase")]
    public float loseRange;
    [Header("When the NPC will open fire/Melee attack")]
    public float attackRange;
    public int health;
    public float fireCooldown;
    public GameObject Player;
    public GameObject Bullet;
    public LayerMask LineOfSightMask;
    public Transform[] patrolPoints;
    [HideInInspector]
    public NavMeshAgent navMesh;
    [HideInInspector]
    private int currWaypoint;
    public bool pcSighted;
    private bool canFire = true;

    public bool CanSeePlayer(GameObject _npc ,Vector3 rayStart, GameObject target)
    {
        RaycastHit hit;
        Vector3 rayDir = target.transform.position - _npc.transform.position;

        Debug.DrawRay(_npc.transform.position, rayDir, Color.red);

        if (Physics.Raycast(_npc.transform.position, rayDir, out hit, sightRange))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    public bool CanAttack(GameObject _target, GameObject _npc, float _loseRange, float _attackRange)
    {
        float distance = Vector3.Distance(_target.transform.position, _npc.transform.position);

        if (distance > _attackRange)
            return false;
        else
            return true;       
    }

    public virtual void Attack(GameObject _npc, GameObject target)
    {
        Vector3 _targetPos = target.transform.position;
        Vector3 _npcPos = _npc.transform.position;

        Vector3 _target = _targetPos - _npcPos;
        Quaternion _qt_rotation = Quaternion.LookRotation(_target);
        _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, _qt_rotation, Time.deltaTime * 6);

        if (canFire)
        {
            canFire = false;
            GameObject _clone;
            _clone = Instantiate(Bullet, _npc.transform.TransformPoint(Vector3.forward), _npc.transform.rotation) as GameObject;
            StartCoroutine(ShootingCooldown());
        }

    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    public void Patrol(GameObject _npc)
    {
        if (currWaypoint < patrolPoints.Length)
        {
            Vector3 targetPos = patrolPoints[currWaypoint].position;
            targetPos.y = _npc.transform.position.y;
            navMesh.SetDestination(targetPos);

            if (Vector3.Distance(_npc.transform.position, targetPos) < 0.5)
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
