using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTarget : MonoBehaviour
{

    private static List<SpellTarget> spellTargets;
    private Mage targetMage = null;
    private int destructableHP;
    private Mage activeCastingMage = null;
    private bool isDamageable;

    public bool IsDamageable
    {
        get => isDamageable;
        set => isDamageable = value;
    }

    public Mage TargetMage => targetMage;

    public static SpellTarget GetClosest(Vector3 position, float maxRange)
    {
        SpellTarget closest = null;
        foreach (var spellTarget in spellTargets)
        {
            if (spellTarget.isActiveAndEnabled)
            {
                if (Vector3.Distance(position, spellTarget.GetPosition()) <= maxRange)
                {
                    if (closest == null)
                    {
                        closest = spellTarget;
                    }
                    else
                    {
                        if (Vector3.Distance(position, spellTarget.GetPosition()) <
                            Vector3.Distance(position, closest.GetPosition()))
                        {
                            closest = spellTarget;
                        }
                    }
                }   
            }
           
        }
        return closest;
    }
    
    private Animator _animator;
    //[SerializeField] private Sprite _damageSprite;
    

    // private void Awake() {
    //     if (spellTargets == null) spellTargets = new List<SpellTarget>();
    //     spellTargets.Add(this);
    //     if (gameObject.CompareTag("MageBody"))
    //     {
    //         _animator = gameObject.GetComponentInChildren<Animator>();
    //         Debug.Log("Renderer loaded" + _animator.ToString());
    //     }
    //     
    // }
    private void Awake()
    {
        isDamageable = false;
        if (gameObject.CompareTag("MageBody"))
        {
            targetMage = gameObject.GetComponent<Mage>();
        }

        if (gameObject.CompareTag("DestructableObject"))
        {
            destructableHP = 2;
            isDamageable = true;
        }
        if (spellTargets == null) spellTargets = new List<SpellTarget>();
        spellTargets.Add(this);
        _animator = gameObject.GetComponentInChildren<Animator>();
        Debug.Log("Renderer loaded" + _animator.ToString());
        this.enabled = true;
    }

    // private void OnEnable()
    // {
    //     if (spellTargets == null) spellTargets = new List<SpellTarget>();
    //     spellTargets.Add(this);
    //     _animator = gameObject.GetComponentInChildren<Animator>();
    //     Debug.Log("Renderer loaded" + _animator.ToString());
    //     
    // }

    // private void OnDisable()
    // {
    //     if (!spellTargets.Contains(this))
    //     {
    //         
    //     }
    //     else
    //     {
    //         spellTargets.Remove(this);  
    //     }
    // }


    public void Damage(Spell spell, Mage castingMage)
    {
        this.activeCastingMage = castingMage;
        CheckMagesInRange(spell);
        Debug.Log("PLaying damage sprite");
    }

    public void PlayDamageAnim()
    {
        if (gameObject.CompareTag("DestructableObject"))
        {
            destructableHP--;
        }
        if (destructableHP == 0 && !gameObject.CompareTag("MageBody"))
        {
            Block[,] mapBlocks = GameObject.Find("BattleMap").GetComponent<MapManager>().MapBlocks;
            foreach (var block in mapBlocks)
            {
                if (block.BlockPosition == this.GetPosition())
                {
                    block.WalkedOff();
                }
            }
            _animator.SetTrigger("Destroy");
            spellTargets.Remove(this);
        }
        else
        {
            _animator.SetTrigger("Damaged"); 
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void DestroyTarget()
    {
        Destroy(gameObject);
    }

    public void ResetTargetList()
    {
        spellTargets.Clear();
    }

    public void CheckMagesInRange(Spell spell)
    {
        Debug.Log("Selected spell to cast (spellTarget) = " + spell.name);
        List<SpellTarget> targetsToDamage = new List<SpellTarget>();
        if (spell.areaOfEffect > 1)
        {
            foreach (SpellTarget target in spellTargets)
            {
               // if (target.CompareTag("MageBody"))
                //{
                float xDistance = 0;
                float yDistance = 0;
                double blockDistance = 0;
                // xDistance = (int) Math.Abs(_activeMage.BlockPosition.x - targetMage.BlockPosition.x); MOVEMENT DISTANCE CALCULATION
                // yDistance = (int) Math.Abs(_activeMage.BlockPosition.y - targetMage.BlockPosition.y);
                xDistance = (float) Math.Pow(this.GetPosition().x - target.GetPosition().x, 2);// *0.64f;
                yDistance = (float) Math.Pow(this.GetPosition().y - target.GetPosition().y, 2);// * 0.64f;
                blockDistance = (Math.Ceiling(Math.Sqrt(xDistance + yDistance)));
                Debug.Log(target.gameObject + "was " + blockDistance + " blocks from the explosion");
                
                   // float distance = Vector3.Distance(this.GetPosition(), target.GetPosition());
                   // float distance = Vector3.Distance(this.targetMage.transform.position, target.targetMage.transform.position) * 0.64f;
                    if (blockDistance < spell.areaOfEffect)
                    {
                        targetsToDamage.Add(target);
                        //target.PlayDamageAnim();
                        if (target.targetMage!=null)
                        {
                            target.targetMage.TakeDamage((int) (spell.spellPower * ((activeCastingMage._abilityPower / 100) + 1)));
                        }
                    }
                //}
                // else
                // {
                //     float distance = Vector3.Distance(this.GetPosition(), target.GetPosition());
                //     if (distance < spell.areaOfEffect)
                //     {
                //         targetsToDamage.Add(target);
                //         //target.PlayDamageAnim();
                //     }
                // }
            }
        }
        else
        {
            PlayDamageAnim();
            if (targetMage != null)
            {
                targetMage.TakeDamage((int) (spell.spellPower * ((activeCastingMage._abilityPower / 100) + 1)));
            }
        }

        foreach (var target in targetsToDamage)
        {
            target.PlayDamageAnim();
        }
        
    }

  
}
