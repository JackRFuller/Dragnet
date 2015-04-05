using UnityEngine;
using System.Collections;

public class GenericNPC : NpcClass, ITakeDamage {


    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        navMesh = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        bool canSeePC = CanSeePlayer(transform.position, Player);

        if (canSeePC)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < loseRange)
            {
                navMesh.SetDestination(Player.transform.position);
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        GetComponentInChildren<TextMesh>().text = "" + health;

        if (health <= 0)
            Destroy(gameObject);
    }
	
}
