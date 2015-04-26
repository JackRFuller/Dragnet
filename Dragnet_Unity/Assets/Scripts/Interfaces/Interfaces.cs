using UnityEngine;
using System.Collections;

public interface IEditable
{
    void EditWeapon(int _ammo, int _maxAmmo, int _clipSize, float _range, float _accuracy, float _fireRate, int _damage, WeaponClass.WeaponType _weaponType, int _shots, float _sprayRadius);
}

public interface ITakeDamage
{
    void TakeDamage(int _damage);
}
