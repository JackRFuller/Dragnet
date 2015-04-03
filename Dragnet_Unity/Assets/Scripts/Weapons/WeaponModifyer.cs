using UnityEngine;
using System.Collections;

public class WeaponModifyer : MonoBehaviour {

    public int ammo;
    public int maxAmmo;
    [Range(0, 200)]
    public int clipSize;
    public float range;
    [Range(0, 0.2f)]
    public float accuracy;
    [Range(0, 5f)]
    public float fireRate;
    public int damage;

    [Header("Graphics/Audio")]
    public Mesh weaponMesh;
    public AudioClip[] weaponAudio;
    public ParticleSystem barrelFire;

    void OnCollisionEnter(Collision other)
    {
        var Editable= (IEditable)other.collider.gameObject.GetComponent(typeof(IEditable));

        if (Editable == null)
            return;

        Editable.EditWeapon(ammo, maxAmmo, clipSize, range, accuracy, fireRate, damage);
    }
}
