using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

    public float DestroyCountdown;
	// Use this for initialization
	void Start () {

        Destroy(gameObject, DestroyCountdown);
	
	}
}
