using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnSpellHover : MonoBehaviour
{
    public GameObject infoDialogue;
    private GameObject instantiatedSpellInfoBox;

    private TextMeshProUGUI _description;
   
    private bool mouseEntered = false;
    private Spell selectedSpell;
    private BattleManager activeManager;
    private void OnMouseEnter()
    {
        activeManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
        mouseEntered = true;
        FindObjectOfType<MusicLibrary>().PlayHoverSound();
        showInfo();
    }


    private void showInfo()
    {
        Bounds descriptionRectTransform = new Bounds();
        RectTransform textSize = new RectTransform();
        List<RectTransform> boxItems = new List<RectTransform>();
        GameObject mainCanvas = GameObject.FindWithTag("Canvas");
        instantiatedSpellInfoBox = Instantiate(infoDialogue, mainCanvas.transform);
        instantiatedSpellInfoBox.transform.position =
            new Vector3(14.69f, 7.21f,
                0f); //(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);

        foreach (Transform eachChild in instantiatedSpellInfoBox.transform)
        {
            if (eachChild.name == "SpellInfoText")
            {
                _description = eachChild.GetComponent<TextMeshProUGUI>();
                textSize = eachChild.GetComponent<RectTransform>();
            }
            else boxItems.Add(eachChild.GetComponent<RectTransform>());
        }


        if (gameObject.name == "Spell1")
        {
            selectedSpell = activeManager.ActiveMage.SpellA;
        }

        else if (gameObject.name == "Spell2")
        {
            selectedSpell = activeManager.ActiveMage.SpellB;
        }

        else if (gameObject.name == "BasicAttack")
        {
            selectedSpell = activeManager.ActiveMage.basicSpell;
        }

        _description.text = selectedSpell.ToString();
        
        _description.ForceMeshUpdate();
        descriptionRectTransform = _description.textBounds;
        textSize.sizeDelta = new Vector2(textSize.sizeDelta.x, descriptionRectTransform.size.y);
        foreach (var item in boxItems)
        {
            item.sizeDelta = new Vector2(item.sizeDelta.x, descriptionRectTransform.size.y  + 20f);
            //item.gameObject.transform.position = _description.gameObject.transform.position; 
            //item.ForceMeshUpdate();
        }
    }

    private void OnMouseExit()
    {
       Destroy(instantiatedSpellInfoBox);
    }
    
}
