using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageFindingTargetState : MageBaseState
{
    private Spell clickedSpell;
    private GameObject selectedTarget;
   // private bool targetClicked = false;
    private MageController activeMController;
    private Vector3 spellDirection;
    private GameObject spellBody;
    public override void EnterState(MageController mageController)
    {
        clickedSpell = mageController.BattleManager.ClickedSpell;
        //targetClicked = false;
        activeMController = mageController;
        mageController.MageHasBeenClicked = false;
        mageController.SpellWasCast = false;
    }

    public override void Update(MageController mageController)
    {
        if (mageController.MageHasBeenClicked)
        {
            mageController.MageHasBeenClicked = false;
            GameObject.Find("CancelButton").SetActive(false);
            SpellTrigger();
            mageController.SpellCast();
            
        }

        if (spellBody != null)
        {
            spellBody.transform.position += spellDirection * (5 * Time.deltaTime);
        }
        
    }

    private void SpellTrigger()
    {
        Cursor.SetCursor(activeMController.BattleManager.DefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        clickedSpell.TriggerSpell(activeMController.Mage);
        spellBody = GameObject.FindWithTag("SpellBody");
        double angleOfXtoY = 0;
        activeMController.BattleManager.FxPlayer.PlayOneShot(activeMController.BattleManager.SpellCastSound);

        activeMController.SpellWasCast = true;
        GameObject spellTarget = activeMController.MageClicked1;
        GameObject spellStart = activeMController.MageGameObject;
        float yDistance = spellTarget.transform.position.y - spellStart.transform.position.y;
        float xDistance = spellTarget.transform.position.x - spellStart.transform.position.x;
        spellBody.transform.position = spellStart.transform.position;
        if (spellTarget.transform.position.x < spellStart.transform.position.x)
        {
            angleOfXtoY = (Math.Atan(yDistance / xDistance) * 100);
        }
        else
        {
            angleOfXtoY = (Math.Atan(yDistance / xDistance) * 100) +180f;  
        }
        

        Debug.Log("" + angleOfXtoY);
        //Vector3 spellDirection = (spellStart.transform.position - ).normalized;
        //spellBody.LeanRotateAroundLocal(spellDirection, 0f);
        spellBody.LeanRotateZ((float) angleOfXtoY, 0f);
        //spellBody.LeanRotateY(0, 0f);
        //spellBody.transform.LookAt(spellTarget.transform);
        //spellBody.transform.rotation = new Quaternion(spellBody.transform.localRotation.x, spellBody.transform.localRotation.y, spellBody.transform.localRotation.z -180f, 0);
        spellDirection = (spellTarget.transform.position - spellBody.transform.position).normalized;
        // Vector3 currentOffset = spellBody.transform.position;
        // Vector3 desiredOffset = spellTarget.transform.position;
        // spellBody.transform.localRotation *= Quaternion.FromToRotation(currentOffset, desiredOffset);
        //spellBody.transform.localRotation -= 180f;
        //spellBody.LeanMove()
        //LeanTween.move(spellBody, spellTarget.transform.position, 3f);
        

    }

    
}
