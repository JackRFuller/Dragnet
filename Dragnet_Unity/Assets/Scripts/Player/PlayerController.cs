using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IEditable {

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

    public WeaponClass weaponClass;
    public PlayerClass playerClass;
    [SerializeField] LineRenderer gunLineRenderer;
   
	// Use this for initialization
	void Start () {

        PC_Animation = transform.FindChild("PC_Mesh").GetComponent<Animator>();
        OriginalSpeed = Speed;
        gunLineRenderer = GetComponentInChildren<LineRenderer>();
        TurnOffLineRenderer();
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
        if (Input.GetMouseButton(0))
            Shoot();

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


    #region Shooting Methods

    private void Shoot()
    {
        if (weaponClass.canFire)
        {
            if (weaponClass.Ammo > 0)
            {
                RaycastHit hit;
                Vector3 fireDirection = transform.TransformDirection(Vector3.forward);
                weaponClass.canFire = false;
                weaponClass.ammoInClip -= 1;

                if (Physics.Raycast(transform.position, fireDirection * weaponClass.range, out hit))
                {
                    InitLineRenderer(hit.point);

                    if (weaponClass.hitParticle != null)
                    {
                        GameObject hitEffect;
                        hitEffect = Instantiate(weaponClass.hitParticle, hit.point, transform.rotation) as GameObject;
                    }
                }
                else
                {
                    InitLineRenderer(transform.TransformDirection(Vector3.forward) * weaponClass.range);
                }

                StartCoroutine(weaponClass.Cooldown());
            }
            else
            {
                Reload();
            }
        }
        else
        {
            TurnOffLineRenderer();
        }
    }

    private void Reload()
    {
        weaponClass.Reload();
    }
    private void InitLineRenderer(Vector3 _hitPoint)
    {
        gunLineRenderer.enabled = true;
        gunLineRenderer.SetPosition(0, transform.position);
        gunLineRenderer.SetPosition(1, _hitPoint);
    }
    private void TurnOffLineRenderer()
    {
        gunLineRenderer.SetPosition(1, transform.position);
        gunLineRenderer.SetPosition(0, transform.position);
        gunLineRenderer.enabled = false;
    }
    #endregion

    #region Interfaces
    public void EditWeapon(int _ammo, int _maxAmmo, int _clipSize, float _range, float _accuracy, float _fireRate, int _damage)
    {
        weaponClass.Ammo = _ammo;
        weaponClass.maxAmmo = _maxAmmo;
        weaponClass.clipSize = _clipSize;
        weaponClass.range = _range;
        weaponClass.accuracy = _accuracy;
        weaponClass.fireRate = _fireRate;
        weaponClass.damage = _damage;

        weaponClass.ammoInClip = weaponClass.clipSize;
        weaponClass.Ammo -= weaponClass.ammoInClip;
    }
    #endregion


    void OnGUI()
    {
        float rx = Screen.width / 1980f;
        float ry = Screen.height / 1080f;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUI.contentColor = Color.yellow;
        GUI.Box(new Rect(1800, 900, 150, 50), "Ammo: " + weaponClass.ammoInClip + " / " + weaponClass.Ammo);
    }
}
