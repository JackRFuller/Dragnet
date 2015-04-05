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
        navMesh.SetDestination(Player.transform.position);
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
    }
	
}
