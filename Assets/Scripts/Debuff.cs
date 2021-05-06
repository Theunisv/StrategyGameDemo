using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public abstract class Debuff : MonoBehaviour
{
    protected string debuffName;
    protected int baseDuration;
    protected int durationLeft;
    protected int effectPotency;
    protected enum debuffTypes{TickEachTurn, TickOnPlayerTurn}
    protected debuffTypes debuffType;
    
    #region Get&Set

    

    public string DebuffName
    {
        get => debuffName;
        set => debuffName = value;
    }

    public int BaseDuration
    {
        get => baseDuration;
        set => baseDuration = value;
    }

    public int DurationLeft
    {
        get => durationLeft;
        set => durationLeft = value;
    }

    public int EffectPotency => effectPotency;

    #endregion

    protected Debuff()
    {
        
    }
    public void ApplyDebuff(Mage targetMage)
    {
        targetMage.Debuffs.Add(this);
    }

    public abstract Debuff ModifyDebuff(Spell spell);
    public abstract void DebuffTick(Mage targetMage);

    public bool DebuffEnd()
    {
        return durationLeft <= 0;
    }

    public void Cleanse()
    {
        durationLeft = 0;
    }
}
