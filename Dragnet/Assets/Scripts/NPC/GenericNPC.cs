﻿using UnityEngine;
using System.Collections;

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
                navMesh.Resume();
                navMesh.SetDestination(Player.transform.position);                  
            }
            else
            {
                navMesh.Stop();
                Attack(gameObject, Player);
            }

            if (_distance > loseRange)
                pcSighted = false;
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
