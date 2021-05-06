using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MageIdleState : MageBaseState
{
    public override void EnterState(MageController mageController)
    {
        foreach (var state in mageController.AnimationStates)
        {
            mageController.MageAnim.SetBool(state, false);
        }
        mageController.MageAnim.SetBool("Idle", true);
        
    }

    public override void Update(MageController mageController)
    {
        mageController.MageAnim.SetBool("Idle", true);
    }

}
