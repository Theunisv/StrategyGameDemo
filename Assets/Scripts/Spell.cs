using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string splName = "New Spell";
    public Sprite splSprite;
    public AudioClip splSound;
    public int splBaseCoolDown = 1;
    public int splRemainingCooldown = 0;
    public bool canTargetAllies;
    public bool canTargetEnemies;
    public int areaOfEffect;
    public string splDescription;
    public int splManaCost;
    public int spellRange;
    public Sprite SplSprite => splSprite;
    public GameObject splPrefab;
    public int spellPower = 20;
    public string DebuffString = "";
    [SerializeField]
    public GameObject debuffPreFab;

    public GameObject explosionPreFab;
    private Debuff debuffToApply;
    
    public int splDuration;

    protected Debuff[] debuffObjects = new Debuff[3];
    public int SpellRange => spellRange;

    public int SplRemainingCooldown
    {
        get => splRemainingCooldown;
        set => splRemainingCooldown = value;
    }

    public GameObject SplPrefab
    {
        get => splPrefab;
        set => splPrefab = value;
    }
 
    public abstract void InitializeSpl();
    public abstract void TriggerSpell(Mage castingMage); //Sets what the spell does
    
    public override string ToString()
    {
        string sAreaOfEffect = (areaOfEffect > 1 ? areaOfEffect.ToString() : "Single target");
        string spellInfo = splDescription + "\nSpell power: " + spellPower + " Mana cost: " + splManaCost
                           + "\nSpell effect area: " + sAreaOfEffect;
        if (debuffToApply != null)
        {
            debuffToApply = debuffToApply.ModifyDebuff(this);
            spellInfo += "\n"+ debuffToApply.DebuffName + " duration: " + debuffToApply.BaseDuration;
        }
        return spellInfo;
    }

    public void GoOnCD()
    {
        splRemainingCooldown = splBaseCoolDown;
    }

    public void TickCDDown()
    {
        if (splRemainingCooldown > 0)
        {
            splRemainingCooldown--;
        }
    }

}

