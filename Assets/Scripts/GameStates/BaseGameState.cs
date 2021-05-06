using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameState
{
    public abstract IEnumerator EnterState(BattleManager gameController);

    public abstract IEnumerator Update(BattleManager gameController);
}
