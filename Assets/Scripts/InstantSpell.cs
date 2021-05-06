using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/InstantSpell", fileName = "InstantSpell")]
public class InstantSpell : Spell
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
        // if (DebuffString != "None"  && debuffToApply != null)
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

    public override void TriggerSpell(Mage castingMage)
    {
        
    }
}
