using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcClass : MonoBehaviour
{
    public enum Behaviours
    {
        Idle,
        Aggressive,
        Enraged
    }
    public Behaviours npcBehaviours;
    public bool SHOWATTACKRAYCASTS;
    public bool SHOWLINEOFSIGHTRAYCASTS;
    public float manualSpeed;
    public int damage;
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
    protected bool canShoot = true;

    ConeEyes eyes;
    VisionCone visionCone;

    public bool CanSeePlayer(GameObject _npc ,Vector3 rayStart, GameObject target)
    {
        RaycastHit hit;
        Vector3 rayDir = (target.transform.position - _npc.transform.position).normalized;
        float distance = Vector3.Distance(_npc.transform.position, target.transform.position);
        Vector3 crossResult = Vector3.Cross(_npc.transform.position, (rayDir * distance));


        if (Physics.Raycast(_npc.transform.position, rayDir, out hit, distance))
        {
            if (SHOWLINEOFSIGHTRAYCASTS)
                Debug.DrawRay(_npc.transform.position, rayDir * distance, Color.red);
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }


        var directionRight = (rayDir * distance) + crossResult.normalized * 0.5f;
        var directionLeft = (rayDir * distance) + crossResult.normalized * -0.5f;

        if (Physics.Raycast(_npc.transform.position, directionRight, out hit, distance))
        {
            if(SHOWLINEOFSIGHTRAYCASTS)
                Debug.DrawRay(_npc.transform.position, directionRight, Color.green);
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        if (Physics.Raycast(_npc.transform.position, directionLeft, out hit, distance))
        {
            if (SHOWLINEOFSIGHTRAYCASTS)
                Debug.DrawRay(_npc.transform.position, directionLeft, Color.green);
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;
        return Vector3.Cross(side1, side2).normalized;
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

        if (canShoot)
        {
            canShoot = false;
            GameObject _clone;
            _clone = Instantiate(Bullet, _npc.transform.TransformPoint(Vector3.forward), _npc.transform.rotation) as GameObject;
            StartCoroutine(AttackCooldown());
        }

    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
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

    public void ManualMove(GameObject _npc, GameObject _target)
    {
        Vector3 moveDir = _target.transform.position - _npc.transform.position;
        moveDir.y = _npc.transform.position.y;
        transform.LookAt(_target.transform);
        Rigidbody _npcRigid = _npc.GetComponent<Rigidbody>();
        _npcRigid.MovePosition(_npcRigid.position + _npc.transform.forward * manualSpeed * Time.deltaTime);
    }

    public GameObject FindNewTarget(GameObject _caller)
    {
        Collider[] _allColliders = Physics.OverlapSphere(_caller.transform.position, 30f);
        List<GameObject> _npcsInRange = new List<GameObject>();

        int i = 0;
        while (i < _allColliders.Length)
        {
            if (_allColliders[i].tag == "Enemy" && _allColliders[i].gameObject != _caller)
            {
                _npcsInRange.Add(_allColliders[i].gameObject);
            }
            i++;
        }

        if (_allColliders.Length == 0)
            return null;

        if (_npcsInRange.Count > 0)
        {
            _npcsInRange.Sort(delegate(GameObject obj1, GameObject obj2)
            {
                return Vector3.Distance(_caller.transform.position, obj1.transform.position).CompareTo
                    ((Vector3.Distance(_caller.transform.position, obj2.transform.position)));
            });

            return _npcsInRange[0];
        }
        else
            return null;

    }

    public void ConeLightOfSight()
    {
        bool playerInView = false;

        foreach (RaycastHit hit in eyes.hits)
        {
            if (hit.transform && hit.transform.tag == "Player")
            {
                playerInView = true;
            }
        }

        if (playerInView)
        {
            visionCone.status = VisionCone.Status.Alert;
        }
        else
        {
            visionCone.status = VisionCone.Status.Idle;
        }
    }
}
