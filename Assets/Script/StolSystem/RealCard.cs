using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RealCard
{
    public string Name;

    public int MeleeDMG;
    public int ShotDMG;
    public int NoArmorDMG;
    public int ArmorBreakerDMG;

    public int Hp;
    public int Helmet;
    public int Shild;
    public int Armor;

    public int Agility1;
    public int Agility2;
    public int Agility3;
    public int Agility4;

    public int Mana;

    public int Line;
    public int Slot;
    public int Position;
    public int Team;
    public int Id;
    public Hiro HiroMain;

    public RealCard Twin;

    public TMP_Text Ui;

    public int MovePoint;
    public List<int> Action;
    public List<int> ShotAction;
    public List<int> PasiveAction;

    public List<string> Trait;


    public List<Effect> Effect;

    public GameObject Body;

}
