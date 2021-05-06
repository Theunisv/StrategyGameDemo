using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTakingDamageState : MageBaseState
{
    public override void EnterState(MageController mageController)
    {
        mageController.MageRenderer.sprite = mageController.DamageSprite;
        mageController.DelayedTransition(mageController.IdleState);
    }

    public override void Update(MageController mageController)
    {
        throw new System.NotImplementedException();
    }
}
