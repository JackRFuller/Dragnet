using UnityEngine;
using System.Collections;

public interface IEditable
{
    void EditWeapon(int _ammo, int _maxAmmo, int _clipSize, float _range, float _accuracy, float _fireRate, int _damage);
}
