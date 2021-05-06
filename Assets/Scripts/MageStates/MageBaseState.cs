using UnityEngine;

public  abstract class MageBaseState
{
    public abstract void EnterState(MageController mageController);

    public abstract void Update(MageController mageController);
}
