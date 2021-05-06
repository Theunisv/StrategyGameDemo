using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellClicked : MonoBehaviour
{
    private BattleManager _activeManager;

    private void Start()
    {
        _activeManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
    }

    private void OnMouseDown()
    {
        Debug.Log("you've clicked " + gameObject.name);
        _activeManager.SpellSelected(gameObject.name);
    }
}
