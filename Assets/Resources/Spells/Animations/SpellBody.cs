using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBody : MonoBehaviour
{
    [SerializeField] private GameObject _endTurnButton;
    
    [SerializeField]
    private int _health;
    [SerializeField]
    private GameObject _healthBar;
    [SerializeField]
    private Collision _collision;

    private Spell _spell;
    private Mage _castingMage;

    public Mage CastingMage
    {
        get => _castingMage;
        set => _castingMage = value;
    }

    public Spell Spell
    {
        get => _spell;
        set => _spell = value;
    }

    private void Awake()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");

        foreach (Transform eachChild in canvas.transform)
        {
            if (eachChild.name == "EndTurnButton 1")
            {
                _endTurnButton = eachChild.gameObject;
            }
        }
    }

    private void Update()
    {
        float hitBox = 0.64f;
        SpellTarget spellTarget = SpellTarget.GetClosest(transform.position, hitBox);
        Debug.Log("Closest spell target : " + spellTarget);
        if (spellTarget != null)
        {
            if (spellTarget.gameObject.CompareTag("MageBody"))
            {
                if (spellTarget.TargetMage != _castingMage && spellTarget.TargetMage.Team != CastingMage.Team)
                {
                    Destroy(gameObject);
                    spellTarget.Damage(_spell , _castingMage);
                    _spell.GoOnCD();
                    _endTurnButton.SetActive(true);
                    if (_spell.areaOfEffect == 1)
                    {
                        GameObject explosion = Instantiate(_spell.explosionPreFab, new Vector3(spellTarget.transform.position.x, spellTarget.transform.position.y + 0.54f, spellTarget.transform.position.z),
                            Quaternion.identity);
                        Destroy (explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.2f); 
                    }

                    if (_spell.areaOfEffect > 1)
                    {
                        GameObject explosion = Instantiate(_spell.explosionPreFab, new Vector3(spellTarget.transform.position.x, spellTarget.transform.position.y, spellTarget.transform.position.z),
                            Quaternion.identity);
                        Destroy (explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.2f); 
                    }
                }
            }
            else
            {
                Destroy(gameObject);
                spellTarget.Damage(_spell , _castingMage);
                _spell.GoOnCD();
                if (_spell.areaOfEffect == 1)
                {
                    GameObject explosion = Instantiate(_spell.explosionPreFab, new Vector3(spellTarget.transform.position.x, spellTarget.transform.position.y + 0.2f, spellTarget.transform.position.z),
                        Quaternion.identity);
                    Destroy (explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.2f); 
                }
                _endTurnButton.SetActive(true); 
            }
            FindObjectOfType<MusicLibrary>().PlaySpellHitSound();
        }
    }


}

