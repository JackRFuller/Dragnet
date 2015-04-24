using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IEditable, ITakeDamage {

    private Animator PC_Animation;

    public enum ControlMethod
    {
        Keyboard,
        SingleStick,
        TwinStick,
        Mouse_Keys
    }

    public ControlMethod ActiveControlScheme;	
    private float OriginalSpeed;

	public Vector3[] LookAtTargets;

    public WeaponClass weaponClass;
    public PlayerClass playerClass = new PlayerClass(100, 100, 100);
    [SerializeField] LineRenderer gunLineRenderer;
    [SerializeField] GameObject lineRendererObject;
    [SerializeField] GameObject Mesh;
    [SerializeField] Text shieldText;
    [SerializeField] Text manaText;
    [SerializeField] Text healthText;
    public LayerMask shootingLayer;
   
	// Use this for initialization
	void Start () {

        PC_Animation = transform.FindChild("PC_Mesh").GetComponent<Animator>();
        OriginalSpeed = playerClass.currSpeed;
        gunLineRenderer = GetComponentInChildren<LineRenderer>();
        PlayerClass.LineOfSight = playerClass.lineOfSight;
        DistanceLogger.range = PlayerClass.LineOfSight;
        TurnOffLineRenderer();
	}
	
	// Update is called once per frame
	void Update () {

        MovementController();
        SwitchControlScheme();
        ManagePlayerSystems();
	}


    private void ManagePlayerSystems()
    {
        if (playerClass.canChargeShield)
        {
            playerClass.RechargeShieldCall(playerClass.shieldRechargeAmount);

            if (!playerClass.canChargeShield)
                StartCoroutine(playerClass.RechargeShieldDelay());

            shieldText.text = "Shield: " + playerClass.Shield;

        }

        if (playerClass.canChargeMana)
        {
            if (playerClass.canChargeMana)
            {
                playerClass.RechargeManaCall(playerClass.manaRechargeAmount);

                if (!playerClass.canChargeMana)
                    StartCoroutine(playerClass.RechargeManaDelay());
            }

            manaText.text = "Mana: " + playerClass.Mana;
        }
        
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
        if (Input.GetMouseButton(0) || Input.GetAxis("R_Trigger") > 0)
        {
            Shoot();
        }
        else
        {
            TurnOffLineRenderer();
        }
            

        if (Input.GetKey("left shift") || Input.GetButton("A") || Input.GetButton("LAP"))
        {
            playerClass.currSpeed = playerClass.sprintSpeed;
            
        }
        else
        {
            playerClass.currSpeed = OriginalSpeed;
            
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (ActiveControlScheme == ControlMethod.Keyboard || ActiveControlScheme == ControlMethod.SingleStick)
        {   
            //if(x != 0 || z != 0)
                transform.rotation = Quaternion.LookRotation(new Vector3(x, 0, z));
        }

        if (ActiveControlScheme == ControlMethod.TwinStick)
        {
            float Rot_X = Input.GetAxis("R_Horizontal");
            float Rot_Z = Input.GetAxis("R_Vertical");

            transform.rotation = Quaternion.LookRotation(new Vector3(Rot_X, transform.rotation.eulerAngles.y, -Rot_Z));
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
               
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Terrain")
                    {
                        Vector3 _target = hit.point - transform.position;
                        _target.y = transform.position.y;
                        transform.rotation = Quaternion.LookRotation(_target.normalized);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                }
             }
         }

        GetComponent<Rigidbody>().velocity = new Vector3(x * playerClass.currSpeed, 0, z * playerClass.currSpeed);

        AnimationController(x,z);
    }

    void AnimationController(float X_Axis, float Z_Axis)
    {
        if (X_Axis == 0 && Z_Axis == 0)
        {
            if (Input.GetMouseButton(0) || Input.GetAxis("R_Trigger") > 0)
            {

                PC_Animation.SetBool("StandingAim", true);
                PC_Animation.SetBool("Idle", false);
                PC_Animation.SetBool("Walking", false);
                PC_Animation.SetBool("Jogging", false);
            }
            else
            {

                PC_Animation.SetBool("Idle", true);
                PC_Animation.SetBool("StandingAim", false);
                PC_Animation.SetBool("Walking", false);
                PC_Animation.SetBool("Jogging", false);
            }
        }
        else
        {
            if (playerClass.currSpeed == playerClass.sprintSpeed)
            {
                PC_Animation.SetBool("Jogging", true);
                PC_Animation.SetBool("Idle", false);
                PC_Animation.SetBool("Walking", false);
                PC_Animation.SetBool("StandingAim", false);
            }
            else
            {
                PC_Animation.SetBool("Walking", true);
                PC_Animation.SetBool("Idle", false);
                PC_Animation.SetBool("Jogging", false);
                PC_Animation.SetBool("StandingAim", false);
            }
        }
    }


    #region Shooting Methods

    private void Shoot()
    {
        if (weaponClass.canFire)
        {
            if (weaponClass.ammoInClip > 0)
            {
                RaycastHit hit;
                Vector3 fireDirection = Vector3.zero;

                if (TargetLockV2.Target != null)
                    fireDirection = TargetLockV2.Target.transform.position - transform.position;              
                else
                    fireDirection = transform.TransformDirection(Vector3.forward);
                
                weaponClass.canFire = false;

                if (weaponClass.weaponType != WeaponClass.WeaponType.Spray)
                {
                    weaponClass.ammoInClip -= 1;
                    fireDirection.x += Random.Range(-weaponClass.accuracy, weaponClass.accuracy);
                    fireDirection.y += Random.Range(-weaponClass.accuracy, weaponClass.accuracy);
                    fireDirection.z += Random.Range(-weaponClass.accuracy, weaponClass.accuracy);

                    if (Physics.Raycast(transform.position, fireDirection * weaponClass.range, out hit))
                    {
                        InitLineRenderer(hit.point);

                        if (weaponClass.hitParticle != null)
                        {
                            GameObject hitEffect;
                            hitEffect = Instantiate(weaponClass.hitParticle, hit.point, transform.rotation) as GameObject;
                        }

                        var takeDamage = (ITakeDamage)hit.collider.gameObject.GetComponent(typeof(ITakeDamage));

                        if (takeDamage != null)
                        {
                            takeDamage.TakeDamage(weaponClass.damage);
                        }
                    }
                    else
                    {
                        //InitLineRenderer(transform.TransformDirection(Vector3.forward) * weaponClass.range);
                    }

                    StartCoroutine(weaponClass.Cooldown());
                }
                else
                {
                    weaponClass.ammoInClip -= 1;
                    
                    for (int i = 0; i < weaponClass.shots; i++)
                    {                       
                        fireDirection = transform.TransformDirection(Vector3.forward);
                        fireDirection.x += Random.Range(-weaponClass.sprayRadius, weaponClass.sprayRadius);
                        fireDirection.y += Random.Range(-weaponClass.sprayRadius, weaponClass.sprayRadius);
                       // fireDirection.z += Random.Range(-weaponClass.sprayRadius, weaponClass.sprayRadius);

                        if (Physics.Raycast(transform.position, fireDirection * weaponClass.range, out hit))
                        {
                            InitLineRenderer(hit.point);

                            if (weaponClass.hitParticle != null)
                            {
                                GameObject hitEffect;
                                hitEffect = Instantiate(weaponClass.hitParticle, hit.point, transform.rotation) as GameObject;
                            }

                            var takeDamage = (ITakeDamage)hit.collider.gameObject.GetComponent(typeof(ITakeDamage));

                            if (takeDamage != null)
                            {
                                takeDamage.TakeDamage(weaponClass.damage);
                            }
                        }
                    }

                    StartCoroutine(weaponClass.Cooldown());
                }
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
        //gunLineRenderer.enabled = true;
        //gunLineRenderer.SetPosition(0, transform.position);
        //gunLineRenderer.SetPosition(1, _hitPoint);
    }
    private void TurnOffLineRenderer()
    {
        //gunLineRenderer.SetPosition(1, transform.position);
        //gunLineRenderer.SetPosition(0, transform.position);
        //gunLineRenderer.enabled = false;
    }
    #endregion

    #region Interfaces
    public void EditWeapon(int _ammo, int _maxAmmo, int _clipSize, float _range, float _accuracy, float _fireRate, int _damage, WeaponClass.WeaponType _weaponType, int _shots, float _sprayRadius)
    {
        weaponClass.Ammo = _ammo;
        weaponClass.maxAmmo = _maxAmmo;
        weaponClass.clipSize = _clipSize;
        weaponClass.range = _range;
        weaponClass.accuracy = _accuracy;
        weaponClass.fireRate = _fireRate;
        weaponClass.damage = _damage;
        weaponClass.weaponType = _weaponType;
        weaponClass.ammoInClip = weaponClass.clipSize;
        weaponClass.Ammo -= weaponClass.ammoInClip;
        weaponClass.shots = _shots;
        weaponClass.sprayRadius = _sprayRadius;
    }

    public void TakeDamage(int _damage)
    {
        playerClass.DoDamage(_damage);
        healthText.text = "Health: " + playerClass.Health;
        shieldText.text = "Shield: " + playerClass.Shield;
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
