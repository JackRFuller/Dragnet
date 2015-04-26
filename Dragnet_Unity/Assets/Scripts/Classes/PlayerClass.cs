using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerClass
{
    public float currSpeed;
    public float sprintSpeed;
    public float originalSpeed;
    public int shieldRechargeAmount;
    public int manaRechargeAmount;
    public float RechargeTimer;
    public float ShieldRechargeTime;
    protected int health;
    public int Health
    {
        get
        {
            if (health > 100)
                health = 100;
            else if (health < 0)
                health = 0;

            return health;
        }
        set
        {
            health = value;
        }
    }
    protected int shield;
    public int Shield
    {
        get
        {
            if (shield > 100)
                shield = 100;
            if (shield < 0)
                shield = 0;

            return shield;
        }
        set
        {
            shield = value;
        }
    }
    protected int mana;
    public int Mana
    {
        get
        {
            if (mana > 100)
                mana = 100;
            if (mana < 0)
                mana = 0;
            return mana;
        }
        set
        {
            mana = value;
        }
    }
    
    [HideInInspector]
    public bool canChargeShield = true;
    [HideInInspector]
    public bool canChargeMana = true;

    public float lineOfSight;
    public static float LineOfSight;

    public PlayerClass(int _mana, int _shield, int _health)
    {
        Mana = _mana;
        Shield = _shield;
        Health = _shield;
    }

    #region Recharge Methods
    public void RechargeShieldCall(int _shieldAmount)
    {
        if (canChargeShield)
        {
            canChargeShield = false;
            RechargeShield(_shieldAmount);
           
        }
    }

    public void RechargeManaCall(int _manaAmount)
    {
        if (canChargeMana)
        {
            canChargeMana = false;
            RechargeMana(_manaAmount);
        }
    }

    public IEnumerator RechargeShieldDelay()
    {
        yield return new WaitForSeconds(ShieldRechargeTime);
        canChargeShield = true;
    }

    public IEnumerator RechargeManaDelay()
    {
        yield return new WaitForSeconds(RechargeTimer);
        canChargeMana = true;
    }

    private void RechargeShield(int _amount)
    {
       Shield += _amount;
    }

    private void RechargeMana(int _amount)
    {
        Mana += _amount;
    }

    #endregion

    #region Mana Methods
    public void ReduceMana(int _amount)
    {
        if (HasEnoughMana(_amount, Mana))
        {
            Mana -= _amount;
        }
    }

    private bool HasEnoughMana(int _attackCost, int currMana)
    {
        int _calculateMana = currMana - _attackCost;

        if (_calculateMana > 0)
            return true;
        else
            return false;
    }
    #endregion

    public void DoDamage(int _damage)
    {
        if (Shield == 0)
            Health -= _damage;
        else
            Shield -= _damage;
    }
}
