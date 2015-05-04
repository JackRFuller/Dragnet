using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericNPC : NpcClass, ITakeDamage {

    bool targetSet;
   
    
    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        navMesh = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if (!pcSighted)
        {
            bool canSeePC = CanSeePlayer(gameObject, transform.position, Player);
            if (canSeePC)
            {
                pcSighted = true;
            }

            Patrol(gameObject);
        }
        else
        {
            float _distance = Vector3.Distance(transform.position, Player.transform.position);
            bool canAttack = CanAttack(Player, gameObject, loseRange, attackRange);

            if (!canAttack)
            {
                bool canSeePC = CanSeePlayer(gameObject, transform.position, Player);

                if (!canSeePC)
                {
                    navMesh.Resume();
                    navMesh.SetDestination(Player.transform.position);
                }
                else
                {
                    navMesh.Stop();
                    ManualMove(gameObject, Player);
                }
            }
            else
            {
                navMesh.Stop();

                if (canShoot)
                {
                    canShoot = false;
                    Attack(gameObject, Player);
                    StartCoroutine(AttackCooldown());
                }                
            }

            if (_distance > loseRange)
                pcSighted = false;
        }

    }

    public override void Attack(GameObject _npc, GameObject target)
    {
        //ROTATION STUFF
        Vector3 _targetPos = target.transform.position;
        Vector3 _npcPos = _npc.transform.position;

        Vector3 _target = _targetPos - _npcPos;
        Quaternion _qt_rotation = Quaternion.LookRotation(_target);
        _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, _qt_rotation, Time.deltaTime * 6);

        //ATTACK STUFF
        Vector3 startPos = _npc.transform.position;
        Vector3 targetPos = Vector3.zero;
        int angle = 15;

        int startAng = (int)(-angle);
        int finishAng = (int)(angle);

        int gap = (int)(angle / 3);
        RaycastHit hit;

        for (int i = startAng; i < finishAng; i += gap)
        {
            targetPos = (Quaternion.Euler(0, i, 0) * _npc.transform.TransformDirection(Vector3.forward)).normalized * 2f;

            if (Physics.Raycast(startPos, targetPos, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    var takeDamage = (ITakeDamage)hit.collider.gameObject.GetComponent(typeof(ITakeDamage));

                    if (takeDamage != null)
                    {
                        takeDamage.TakeDamage(damage);
                        break;
                    }
                }
            }

            if(SHOWATTACKRAYCASTS)
                Debug.DrawRay(startPos, targetPos, Color.cyan);
        }

    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        GetComponentInChildren<TextMesh>().text = "" + health;

        if (health <= 0)
        {
            Player.GetComponent<TargetLockV2>().npcsInView.Remove(gameObject);

            TargetLockV2.Target = FindNewTarget(gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void OnCollisionStay()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }


	
}
