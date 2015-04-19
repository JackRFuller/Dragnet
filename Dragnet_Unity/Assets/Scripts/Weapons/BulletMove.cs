using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {

    Rigidbody bulletRigidbody;
    public float speed;
    public float damage;
    public float lifeTime;
	// Use this for initialization
	void Start () {

        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.TransformDirection(0, 0, speed);
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
