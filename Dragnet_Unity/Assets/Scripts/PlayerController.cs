using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Animator PC_Animation;

    public enum ControlMethod
    {
        Keyboard,
        SingleStick,
        TwinStick,
        Mouse_Keys
    }

    public ControlMethod ActiveControlScheme;

	[SerializeField] float Speed;
	[SerializeField] float SprintSpeed;
    private float OriginalSpeed;

	public Vector3[] LookAtTargets;

	// Use this for initialization
	void Start () {

        PC_Animation = transform.FindChild("PC_Mesh").GetComponent<Animator>();
        OriginalSpeed = Speed;
	
	}
	
	// Update is called once per frame
	void Update () {

        MovementController();

        SwitchControlScheme();

	}

    void SwitchControlScheme()
    {
        if (Input.GetKeyDown("1"))
        {
            ActiveControlScheme = ControlMethod.SingleStick;
        }
        if (Input.GetKeyDown("2"))
        {
            ActiveControlScheme = ControlMethod.TwinStick;
        }
        if (Input.GetKeyDown("3"))
        {
            ActiveControlScheme = ControlMethod.Mouse_Keys;
        }
    }

    void MovementController()
    {
        if (Input.GetKey("left shift") || Input.GetButton("A"))
        {
            Speed = SprintSpeed;
        }
        else
        {
            Speed = OriginalSpeed;
        }

        float x = x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (ActiveControlScheme == ControlMethod.Keyboard || ActiveControlScheme == ControlMethod.SingleStick)
        {           
            transform.rotation = Quaternion.LookRotation(new Vector3(x, 0, z));
        }

        if (ActiveControlScheme == ControlMethod.TwinStick)
        {
            float Rot_X = Input.GetAxis("R_Horizontal");
            float Rot_Z = Input.GetAxis("R_Vertical");

            transform.rotation = Quaternion.LookRotation(new Vector3(Rot_X, 0, -Rot_Z));
        }

        if (ActiveControlScheme == ControlMethod.Mouse_Keys)
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            float screenX = Screen.width;
            float screenY = Screen.height;

            if ((mouseX > 0 || mouseX < screenX) && (mouseY > 0 || mouseX < screenY))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Terrain")
                    {
                        Vector3 _target = hit.point - transform.position;
                        _target.y += 0.5F;
                        transform.rotation = Quaternion.LookRotation(_target.normalized);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                }
                
            }
         }

        GetComponent<Rigidbody>().velocity = new Vector3(x * Speed, 0, z * Speed);

        AnimationController(x,z);
    }

    void AnimationController(float X_Axis, float Z_Axis)
    {
        if (X_Axis == 0 && Z_Axis == 0)
        {
            PC_Animation.SetBool("Idle", true);
            PC_Animation.SetBool("Walking", false);
            PC_Animation.SetBool("Jogging", false);
           
        }
        else
        {
            if (Speed == SprintSpeed)
            {
                PC_Animation.SetBool("Jogging", true);
                PC_Animation.SetBool("Idle", false);
                PC_Animation.SetBool("Walking", false);
            }
            else
            {
                PC_Animation.SetBool("Walking", true);
                PC_Animation.SetBool("Idle", false);
                PC_Animation.SetBool("Jogging", false);
            }
        }
    }

	
}
