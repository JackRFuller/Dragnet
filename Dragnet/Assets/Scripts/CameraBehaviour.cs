using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	public Transform Target;
	[SerializeField] float X_Offset;
	[SerializeField] float Y_Offset;
	[SerializeField] float Z_Offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (Target.position.x, Target.position.y + Y_Offset, Target.position.z + Z_Offset);
	
	}
}
