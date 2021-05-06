using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffect : Debuff
{
    public RootEffect()
    {
        this.debuffName = "RootEffect";
        this.baseDuration = 3;
        this.durationLeft = 3;
        this.effectPotency = 5;
        this.debuffType = debuffTypes.TickOnPlayerTurn;
    }

    public override Debuff ModifyDebuff(Spell spell)
    {
        this.baseDuration = spell.splDuration;
        this.durationLeft = spell.splDuration;
        return this;
    }

    public override void DebuffTick(Mage targetMage)
    {
        DurationLeft--;
    }
}
