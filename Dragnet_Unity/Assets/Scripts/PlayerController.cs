using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[SerializeField] float Speed;
	[SerializeField] GameObject PC_Mesh;

	public Vector3[] LookAtTargets;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//PC_Mesh.transform.localPosition = new Vector3 (0, 0, 0);

		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis ("Vertical");



		GetComponent<Rigidbody> ().velocity = new Vector3 (x * Speed,0, z * Speed);
		transform.rotation = Quaternion.LookRotation(new Vector3(x,0,z));

		//LockRotation (x,z);

	
	}

	void LockRotation(float X_Axis, float Z_Axist)
	{
		if(X_Axis > 0.1 && Z_Axist == 0)
		{
			transform.LookAt(LookAtTargets[0]);
		}
		if(X_Axis < -0.1 && Z_Axist == 0){
		}
		if(X_Axis > 0 && Z_Axist == 0.1){
		}
		if(X_Axis > 0 && Z_Axist == -0.1){
		}
		if(X_Axis > 0.1 && Z_Axist == 0.1){
		}
		if(X_Axis > -0.1 && Z_Axist == -0.1){
		}
	}
}
