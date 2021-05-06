using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OnUnitHover : MonoBehaviour
{
    public GameObject infoDialogue;
    private GameObject instantiatedInfoBox;

    private TextMeshProUGUI _description;
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI mpText;
    private Image hpMask;
    private Image mpMask;

    private bool mouseEntered = false;
    private void OnMouseEnter()
    {
        mouseEntered = true;
        
        StartCoroutine(showInfo());
    }

    private void setHpMpBar(Mage currMage, int currHp, int currMp, int maxHp, int maxMp)
    {
        float hpFillAmount = (float)currHp / (float)maxHp;
        float mpFillAmount = (float)currMp / (float)maxMp;

        hpMask.fillAmount = hpFillAmount;
        mpMask.fillAmount = mpFillAmount;
        hpText.text = currHp + "/" + maxHp;
        mpText.text = currMp + "/" + maxMp;
        
        
    }
    private void OnMouseExit()
    {
        mouseEntered = false;
        StopCoroutine(showInfo());
        Destroy(instantiatedInfoBox);
        
    }

    private IEnumerator showInfo()
    {
        while (mouseEntered)
        {
            
            Mage currentMage = gameObject.GetComponent<Mage>();
            //currentMage = gameObject.GetComponent<Mage>();
            GameObject mainCanvas = GameObject.FindWithTag("Canvas");
            instantiatedInfoBox = Instantiate(infoDialogue, mainCanvas.transform);
            instantiatedInfoBox.transform.position = new Vector3(14.69f, 7.21f, 0f);//(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
            
            Debug.Log(currentMage.name + " Hovered");
        
            foreach (Transform eachChild in instantiatedInfoBox.transform) {
                if (eachChild.name == "Description")
                {
                    _description = eachChild.GetComponent<TextMeshProUGUI>();
                }
                if (eachChild.name == "HPAmount")
                {
                    hpText = eachChild.gameObject.GetComponent<TextMeshProUGUI>();
                }
                if (eachChild.name == "MPAmount")
                {
                    mpText = eachChild.gameObject.GetComponent<TextMeshProUGUI>();
                }
                if (eachChild.name == "HPBar")
                {
                    hpMask = eachChild.gameObject.GetComponent<Image>();
                }
                if (eachChild.name == "MPBar")
                {
                    mpMask = eachChild.gameObject.GetComponent<Image>();
                }
            }

            _description.text = currentMage.ToString();
        
            setHpMpBar(currentMage, currentMage.CurrentHealth, currentMage.CurrentMana, currentMage.MaxHealth, currentMage.MaxManaPool);
            yield return new WaitForSeconds(0.25f);
            Destroy(instantiatedInfoBox);
        }
    }
}
