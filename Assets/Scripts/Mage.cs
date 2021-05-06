using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    public string _name;
    public int _maxHealth;
    public int _currentHealth;
    public float _abilityPower;
    public float _magicalDefence;
    public float _critChance;
    public float _dodgeChance;
    public int _maxManaPool;
    public int _currentMana;
    public Spell basicSpell;
    public Spell _spellA;
    public Spell _spellB;
    public int _castSpeed;
    public int _team;
    private List<Debuff> _debuffs;
    private Vector2 _blockPosition;

    public Mage()
    {
        
    }

    public Mage(string name, int maxHealth, int currentHealth, int magicalDefence, float abilityPower, float critChance, float dodgeChance, int maxManaPool, int currentMana, Spell spellA, Spell spellB, int castSpeed, int team)
    {
        _name = name;
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _magicalDefence = magicalDefence;
        _abilityPower = abilityPower;
        _critChance = critChance;
        _dodgeChance = dodgeChance;
        _maxManaPool = maxManaPool;
        _currentMana = currentMana;
        _spellA = spellA;
        _spellB = spellB;
        _castSpeed = castSpeed;
        _team = team;
    }
    
    public int CastSpeed => _castSpeed;

    public Spell SpellA => _spellA;

    public Spell SpellB => _spellB;

    public int CurrentHealth => _currentHealth;

    public int CurrentMana
    {
        get => _currentMana;
        set => _currentMana = value;
    }

    public int MaxHealth => _maxHealth;

    public int MaxManaPool => _maxManaPool;

    public List<Debuff> Debuffs
    {
        get => _debuffs;
        set => _debuffs = value;
    }
    public int Team
    {
        get => _team;
        set => _team = value;
    }

    public Vector2 BlockPosition
    {
        get => _blockPosition;
        set => _blockPosition = value;
    }

    public void InitializeSpells()
    {
        _spellA= Instantiate(_spellA);
      _spellB = Instantiate(_spellB);
    }

    public bool IsAlive()
    {
        if (_currentHealth <= 0)
            return false;

        return true;
    }

    public string CanCast(Spell spell)
    {
        if (spell == _spellA && _spellA.splRemainingCooldown <= 0)
        {
            if (_currentMana < _spellA.splManaCost) return "mana";
            return "success";
        }
        else if (spell == _spellB && _spellB.splRemainingCooldown <= 0)
        {
            if (_currentMana < _spellB.splManaCost) return "mana";
            return "success";
        }
        if (spell == basicSpell)
            return "success";
        return "cooldown";
    }
    public string CastSpell(Spell spell)
    {
        if (spell == _spellA && _spellA.splRemainingCooldown <= 0)
        {
            if (_currentMana < _spellA.splManaCost) return "mana";;
            _spellA.splRemainingCooldown = _spellA.splBaseCoolDown;
            _currentMana -= _spellA.splManaCost;
            return "success";
        }
        else if (spell == _spellB && _spellB.splRemainingCooldown <= 0)
        {
            if (_currentMana < _spellB.splManaCost) return "mana";
            _spellB.splRemainingCooldown = _spellB.splBaseCoolDown;
            _currentMana -= _spellB.splManaCost;
            return "success";
        }

        return "cooldown";
    }

    public void TakeDamage(int spellDmg)
    {
        _currentHealth -= spellDmg  - (int)_magicalDefence;
    }


    public void DebuffTick()
    {
        foreach (var debuff in _debuffs)
        {
            debuff.DebuffTick(this);
            if (debuff.DebuffEnd())
            {
                _debuffs.Remove(debuff);
            }
        }
        
    }
    public override string ToString()
    {
        string mageInfo =  "Ability Power : " + _abilityPower + " Crit : " + _critChance;
        mageInfo += "\nDefence : "+ _magicalDefence + "   Dodge :" + _dodgeChance;
        mageInfo += "\nSpell A : " + _spellA.splName + "   \nSpell B : " + _spellB.splName;
        return mageInfo;
    }
}
