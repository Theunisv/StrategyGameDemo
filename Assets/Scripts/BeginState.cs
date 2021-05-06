using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginState : State
{
   public BeginState(BattleManager mageController) : base(mageController)
   {
      
   }
   public override IEnumerator Start()
   {
      return base.Start();
   }
}
