using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class MouseDownTrigger : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject canvas = GameObject.Find("MainMenu Canvas");
        CanvasControls ccontrol = canvas.GetComponent<CanvasControls>();
        
        if (gameObject.name.Contains("Mage"))
        {
            ccontrol.SelectMage(this.gameObject);
            ccontrol.PlayClickSound();
        }
        else if(gameObject.name.Contains("Lock"))
        {
            if (gameObject.name.Equals("Lock 1"))
            {
                ccontrol.LockMage(0);
                ccontrol.PlayClickSound();
            }
            if (gameObject.name.Equals("Lock 2"))
            {
                ccontrol.LockMage(1);
                ccontrol.PlayClickSound();
            }
            if (gameObject.name.Equals("Lock 3"))
            {
                ccontrol.LockMage(2);
                ccontrol.PlayClickSound();
            }
        }
    }

    private void OnMouseEnter()
    {
        GameObject canvas = GameObject.Find("MainMenu Canvas");
        CanvasControls ccontrol = canvas.GetComponent<CanvasControls>();
        
        ccontrol.PlayHoverSound();
    }
}
