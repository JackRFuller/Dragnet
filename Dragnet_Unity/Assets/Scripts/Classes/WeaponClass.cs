using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponClass 
{
    [Header("Weapon Modifyers")]
    private int ammo;
    public int Ammo
    {
        get
        {
            if (ammo > maxAmmo)
                ammo = maxAmmo;

            return ammo;
        }
        set
        {
            ammo = value;
        }
    }
    public int maxAmmo;
    public int clipSize;
    public int ammoInClip;
    public float range;
    [Range(0, 0.2f)]
    public float accuracy;
    public float fireRate;
    public int damage;

    [Header("Graphics/Audio")]
    public Mesh weaponMesh;
    public AudioClip[] weaponAudio;
    public ParticleSystem barrelFire;
    public ParticleSystem hitParticle;

    public bool canFire = true;


    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }


    public void Reload()
    {
        if (Ammo > 0)//if we have ammo
        {
            // Debug.Log("ALL AMMO: " + Ammo);
            if (ammoInClip < clipSize)// if we have used a shot in the clip
            {
                int ammoToReload = clipSize - ammoInClip; // calculate amount of ammo to reload
                // Debug.Log("AMMO NEEDED TO RELOAD: " + ammoToReload);
                int checkAmmoWeHave = Ammo - ammoToReload;
                //Debug.Log("AMMO WE HAVE AFTER RELOAD " + checkAmmoWeHave);
                if (checkAmmoWeHave > 0)
                {
                    //RELOAD
                    Ammo -= ammoToReload;
                    ammoInClip = ammoToReload;
                }
                else
                {
                    //if we didnt have enough the calculate the number we are short then reload whats left
                    ammoToReload = clipSize - Mathf.Abs(checkAmmoWeHave);
                    Ammo -= ammoToReload;
                    ammoInClip += ammoToReload;

                }
            }
        }
    }
}
