using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BurnEffect : Debuff
{
    public BurnEffect()
    {
        this.debuffName = "BurnEffect";
        this.baseDuration = 3;
        this.durationLeft = 3;
        this.effectPotency = 5;
        this.debuffType = debuffTypes.TickEachTurn;
    }

    public override Debuff ModifyDebuff(Spell spell)
    {
        this.baseDuration = spell.splDuration;
        this.durationLeft = spell.splDuration;
        return this;
    }

    public override void DebuffTick(Mage targetMage)
    {
        targetMage._currentHealth -= EffectPotency;
        DurationLeft--;
    }
}
