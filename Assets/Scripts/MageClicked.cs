using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClicked : MonoBehaviour
{
    private BattleManager activeManager;

    private void Start()
    {
        activeManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
    }

    private void OnMouseDown()
    {
        activeManager.CurrentMageController.MageClicked1 = gameObject;
        activeManager.CurrentMageController.MageHasBeenClicked = true;
        //Destroy(this);
    }

    private void Update()
    {
        activeManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
        if (activeManager.CurrentMageController.SpellWasCast)
        {
            Destroy(this);
        }
    }
}
