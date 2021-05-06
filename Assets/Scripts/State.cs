using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
   protected BattleManager MageController;

   public State(BattleManager mageController)
   {
      MageController = mageController;
   }
   public virtual IEnumerator Start()
   {
      yield break;
   }

   public virtual IEnumerator BeginningNewTurn()
   {
      yield break;
   }
   
   public virtual IEnumerator Idle()
   {
      
      yield break;
   }
   
   public virtual IEnumerator Move()
   {
      yield break;   
   }
   
   public virtual IEnumerator CastOffensiveSpell()
   {
      yield break;   
   }
   
   public virtual IEnumerator CastDeffensiveSpell()
   {
      yield break;   
   }
   
   public virtual IEnumerator Die()
   {
      yield break;   
   }
   
   
}
