using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int Damage;
    public int SkillDamage;
    public int maxHP;
    public int minHP;
    public int currentHP;

    public bool isDefending = false;
    public bool isBuffed = false;
    public bool isDebuffed = false;
    public int buffDuration = 0;
    public int debuffDuration = 0;
    public bool TakeDamage(int dmg)
    {
        if (isDefending)
        {
            dmg /= 2; // Mengurangi damage saat defend
        }

        currentHP -= dmg;

        if (currentHP <= 0)
            return true;

        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if(currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void ApplyBuff(int buffAmount, int duration)
    {
        Damage += buffAmount;
        SkillDamage += buffAmount;
        isBuffed = true;
        buffDuration = duration;
    }

    public void ApplyDebuff(int debuffAmount, int duration)
    {
        Damage -= debuffAmount;
        SkillDamage -= debuffAmount;
        isDebuffed = true;
        debuffDuration = duration;
    }

    public void UpdateStatusEffects()
    {
        // Reset Defend status
        isDefending = false;

        // Update Buff
        if (isBuffed)
        {
            buffDuration--;
            if (buffDuration <= 0)
            {
                isBuffed = false;
                Damage -= 5; // Reset buff damage
                SkillDamage -= 5;
            }
        }

        // Update Debuff
        if (isDebuffed)
        {
            debuffDuration--;
            if (debuffDuration <= 0)
            {
                isDebuffed = false;
                Damage += 5; // Reset debuff damage
                SkillDamage += 5;
            }
        }
    }

}
