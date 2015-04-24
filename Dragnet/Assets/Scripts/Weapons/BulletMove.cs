using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {

    Rigidbody bulletRigidbody;
    public float speed;
    public int damage;
    public float lifeTime;
	// Use this for initialization
	void Start () {

        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.TransformDirection(0, 0, speed);
        Destroy(gameObject, lifeTime);
	}

    void OnCollisionEnter(Collision hit)
    {
        var takeDamage = (ITakeDamage)hit.collider.gameObject.GetComponent(typeof(ITakeDamage));

        if (takeDamage != null)
        {
            takeDamage.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
