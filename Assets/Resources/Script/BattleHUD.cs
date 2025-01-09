using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text currentHP;
    public Text maxHP;


    public void setHUD (Unit unit)
    {
        string textMaxHp = unit.maxHP.ToString();
        string textCurrentHP = unit.currentHP.ToString();

        nameText.text = unit.name;
        currentHP.text = textCurrentHP;
        maxHP.text = textMaxHp;
    }

    public void setHP(int hp)
    {
        // Mengupdate teks currentHP
        currentHP.text = hp.ToString();
    }
}
