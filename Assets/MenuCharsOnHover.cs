using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCharsOnHover : MonoBehaviour
{
    [SerializeField] private GameObject spellAInfo;
    [SerializeField] private GameObject spellBInfo;
    [SerializeField] private GameObject charInfo;
    [SerializeField] private GameObject hpMaskGO;
    [SerializeField] private GameObject mpMaskGO;
    
    
    private bool mouseEntered = false;

    private void Awake()
    {
        // spellAInfo = GameObject.Find("SpellAInfoBox");
        // spellBInfo = GameObject.Find("SpellBInfoBox");
        // charInfo = GameObject.Find("CharacterInfo");
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Mage"))
        {
            mouseEntered = true;
            showInfo(hit.collider.GetComponent<Mage>());
        }
        else
        {
            spellAInfo.SetActive(false);
            spellBInfo.SetActive(false);
            charInfo.SetActive(false);
        }
    }

    private void showInfo(Mage hoveredMage)
    {
        Bounds descriptionRectTransform = new Bounds();
        RectTransform textSize = new RectTransform();
        List<RectTransform> boxItems = new List<RectTransform>();
        GameObject mainCanvas = GameObject.FindWithTag("Canvas");
        spellAInfo.SetActive(true);
        spellBInfo.SetActive(true);
        charInfo.SetActive(true);
        
        float hpFillAmount = (float)hoveredMage.CurrentHealth / (float)hoveredMage.MaxHealth;
        float mpFillAmount = (float)hoveredMage.CurrentMana / (float)hoveredMage.MaxManaPool;

        Image hpMask = hpMaskGO.GetComponent<Image>();
        Image mpMask = mpMaskGO.GetComponent<Image>();
        hpMask.fillAmount = hpFillAmount;
        mpMask.fillAmount = mpFillAmount;
        GameObject.Find("HPAmount").GetComponent<TextMeshProUGUI>().text = hoveredMage.CurrentHealth + "/" + hoveredMage.MaxHealth;
        GameObject.Find("MPAmount").GetComponent<TextMeshProUGUI>().text = hoveredMage.CurrentMana + "/" + hoveredMage.MaxManaPool;
        

        TextMeshProUGUI charDescription = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
        charDescription.text = hoveredMage.ToString();
        TextMeshProUGUI spellAText = GameObject.Find("SpellAInfoText").GetComponent<TextMeshProUGUI>();
        spellAText.text = hoveredMage.SpellA.ToString();
        TextMeshProUGUI spellBText = GameObject.Find("SpellBInfoText").GetComponent<TextMeshProUGUI>();
        spellBText.text = hoveredMage.SpellB.ToString();
        
        GameObject.Find("SpellAIcon").GetComponent<Image>().sprite =  hoveredMage.SpellA.SplSprite;
        GameObject.Find("SpellBIcon").GetComponent<Image>().sprite =  hoveredMage.SpellB.SplSprite;
        
        charDescription.ForceMeshUpdate();
        spellAText.ForceMeshUpdate();
        spellBText.ForceMeshUpdate();
        
        Bounds spellABounds = spellAText.textBounds;
        Bounds spellBBounds = spellBText.textBounds;
        RectTransform spellASize = GameObject.Find("SpellAInfoImage").GetComponent<RectTransform>();
        RectTransform spellBSize = GameObject.Find("SpellBInfoImage").GetComponent<RectTransform>();
        
        spellASize.sizeDelta = new Vector2(spellASize.sizeDelta.x, spellABounds.size.y + 20f);
        spellBSize.sizeDelta = new Vector2(spellBSize.sizeDelta.x, spellBBounds.size.y + 20f);

    }

    private void OnMouseExit()
    {
      spellAInfo.SetActive(false);
      spellBInfo.SetActive(false);
      charInfo.SetActive(false);
    }
    
}
