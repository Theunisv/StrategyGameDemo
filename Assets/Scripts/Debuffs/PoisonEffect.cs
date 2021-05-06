using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Debuff
{
    public PoisonEffect()
    {
        this.debuffName = "PoisonEffect";
        this.baseDuration = 4;
        this.durationLeft = 4;
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
