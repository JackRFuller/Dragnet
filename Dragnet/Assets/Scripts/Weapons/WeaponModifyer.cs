using UnityEngine;
using System.Collections;

public class WeaponModifyer : MonoBehaviour {

    //public enum WeaponType
    //{
    //    SingleShot,
    //    Spray,
    //    Sniper
    //}
    public WeaponClass.WeaponType weaponType;

    [Header("The Amount of Ammo given when picked up")]
    public int ammo;
    [Header("Max Amount of Ammo that can be carried")]
    public int maxAmmo;
    [Header("How many shots the gun Clip can hold before reload")]
    [Range(0, 200)]
    public int clipSize;
    public float range;
    [Range(0, 0.2f)]
    public float accuracy;
    [Range(0, 5f)]
    public float fireRate;
    [Header("Damage per bullet, remeber a shotgun fires multiple in a burst")]
    public int damage;

    [Header("Shotgun Variables")]
    [Range(0, 0.5f)]
    public float sprayRadius;
    public int shotsFired;

    [Header("Graphics/Audio")]
    public Mesh weaponMesh;
    public AudioClip[] weaponAudio;
    public ParticleSystem barrelFire;

    void OnCollisionEnter(Collision other)
    {
        var Editable= (IEditable)other.collider.gameObject.GetComponent(typeof(IEditable));

        if (Editable == null)
            return;

        Editable.EditWeapon(ammo, maxAmmo, clipSize, range, accuracy, fireRate, damage, weaponType, shotsFired, sprayRadius);
    }
}
