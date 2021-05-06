using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/ProjectileSpells", fileName = "ProjectileSpells")]
public class ProjectileSpell : Spell
{
   
    //public float spellRange = 5f;
    public Color laserColor = Color.white;

    private RayCastSpellTriggerable rcCast;
    public void Awake()
    {
        // if (DebuffString != "None" && debuffToApply != null)
        // {
        //     foreach (var debuff in allDebuffs)
        //     {
        //         if (debuff.DebuffName == DebuffString)
        //         {
        //             debuffToApply = debuff;
        //             debuffToApply.ModifyDebuff(this);
        //         }
        //     }
        // }
    }
    public override void InitializeSpl()
    {
    }

    public override void TriggerSpell(Mage castingMage)
    {
        GameObject spellObject = Instantiate(splPrefab, new Vector3(-100f, -100f, 0), Quaternion.identity);
        spellObject.name = splName + "Object";
        SpellBody thisSBody = spellObject.GetComponent<SpellBody>();
        thisSBody.Spell = this;
        thisSBody.CastingMage = castingMage;
    }
    
    
}
