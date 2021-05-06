using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D spellCastCursor;
    [SerializeField] private Texture2D moveCursor;

    public Texture2D DefaultCursor => defaultCursor;
    
    
    private GameObject _playerInfo;
    private GameObject _attackingMage;
    private GameObject _targetMage;
    private Spell _spellCast;
    private Spell clickedSpell;
    private List<Mage> magesInRange = new List<Mage>();
    public GameObject stateInformationBox;
    public TextMeshPro informationText;
    [SerializeField] private GameObject spellErrorBox;
    [SerializeField] private AudioSource fxPlayer;
    [SerializeField] private AudioClip spellCastSound;
    [SerializeField] private AudioClip spellHitSound;
    public AudioSource FxPlayer
    {
        get => fxPlayer;
        set => fxPlayer = value;
    }
    public AudioClip SpellCastSound => spellCastSound;
    public AudioClip SpellHitSound => spellHitSound;
    
    private int turnPoints;
    private GameObject moveCDText, basicAttackCDText, spellACDText, spellBCDText;
    private SortOrderHelper _sortOrderHelper;
    [SerializeField]
    private GameObject _cancelButton;

    public Mage ActiveMage => _activeMage;


    public List<Mage> MagesInRange
    {
        get => magesInRange;
        set => magesInRange = value;
    }

    public Spell ClickedSpell
    {
        get => clickedSpell;
        set => clickedSpell = value;
    }

    private State _currentState;

    [SerializeField] private GameObject _movesRemaining;
    
    [SerializeField] private GameObject _damageText;
    [SerializeField] private GameObject _cdTextSpellA;
    [SerializeField] private GameObject _cdTextSpellB;
    [SerializeField] private GameObject fadingToolTip;
    [SerializeField] private GameObject _enemySelectIcon;
    [SerializeField] private GameObject _allySelectIcon;
    [SerializeField] private GameObject _activeMageSelectIcon;
    
    private List<GameObject> _selectArrows = new List<GameObject>();

    public List<GameObject> SelectArrows
    {
        get => _selectArrows;
        set => _selectArrows = value;
    }

    public GameObject DamageText => _damageText;

    public GameObject MovesRemaining => _movesRemaining;
    
    
    
    private MapManager _activeMapManager;
    private GizmoManager _gizmoManager;
    public Mage[] _activeMages = new Mage[6];
    

    private List<Mage> _turnOrder  = new List<Mage>();

    public GizmoManager GizmoManager => _gizmoManager;

    public List<Mage> TurnOrder
    {
        get => _turnOrder;
        set => _turnOrder = value;
    }

    [SerializeField] private GameObject spellBar;
    [SerializeField] private Image[] spellSlots;
    public GameObject moveButton;
    [SerializeField] private Spell basicSpell;

    #region MageInfo

    private Mage _nextMageToCast;
    private Mage _activeMage, _spellTarget, _clickedMage;
    private int _currentMageIndex;
    private int _jumpsRemaining = 4;
    public Animator[] mageAnims = new Animator[6];

    private MageController _currentMageController;

    public MageController CurrentMageController
    {
        get => _currentMageController;
        set => _currentMageController = value;
    }

    #endregion

    void Start()
    {
        _gizmoManager = GameObject.Find("BattleMap").GetComponent<GizmoManager>();
        //InitialiseTurnOrder();
        InitialBattleSetup();
        foreach (var mage in _turnOrder)
        {
            mage.SpellA.InitializeSpl();
            mage.SpellB.InitializeSpl();
        }
        //UpdateSpellBar();
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        
    }

    private void Update()
    {
        informationText = stateInformationBox.GetComponent<TextMeshPro>();
        UpdateCharInfo();

        
//        informationText.text = CurrentMageController.CurrentState.ToString() + "Spell selected:" + clickedSpell;
    }

    private void InitialBattleSetup()
    {
        
    }

    private void UpdateCharInfo()
    {
        GameObject activeInfo = GameObject.FindGameObjectWithTag("MageInfoMain");;
        TextMeshProUGUI hpText= new TextMeshProUGUI();
        TextMeshProUGUI mpText= new TextMeshProUGUI();
        Image hpMask = null;
        Image mpMask = null;
        TextMeshProUGUI mageDescription = new TextMeshProUGUI();
        
        foreach (Transform eachChild in activeInfo.transform) {
            if (eachChild.name == "DescriptionActive")  mageDescription = eachChild.GetComponent<TextMeshProUGUI>();
            if (eachChild.name == "HPAmountActive") hpText = eachChild.gameObject.GetComponent<TextMeshProUGUI>();
            if (eachChild.name == "MPAmountActive") mpText = eachChild.gameObject.GetComponent<TextMeshProUGUI>();
            if (eachChild.name == "HPBarActive") hpMask = eachChild.gameObject.GetComponent<Image>();
            if (eachChild.name == "MPBarActive") mpMask = eachChild.gameObject.GetComponent<Image>();
        }

        mageDescription.text = _activeMage.ToString();
        
        float hpFillAmount = (float)(_activeMage.CurrentHealth ) / (float)_activeMage.MaxHealth;
        float mpFillAmount = (float)_activeMage.CurrentMana / (float)_activeMage.MaxManaPool;

        hpMask.fillAmount = hpFillAmount;
        mpMask.fillAmount = mpFillAmount;
        hpText.text = _activeMage.CurrentHealth  + "/" + _activeMage.MaxHealth;
        mpText.text = _activeMage.CurrentMana  + "/" + _activeMage.MaxManaPool;
    }

    private void UpdateSpellBar(Mage currentMage)
    {
        spellSlots[2].GetComponent<Image>().sprite =  _activeMage._spellA.splSprite;
        spellSlots[3].GetComponent<Image>().sprite =  _activeMage._spellB.splSprite;
        if ( _activeMage.SpellA.SplRemainingCooldown > 0)
        {
            Debug.Log("SpellA still on cd!");
            _cdTextSpellA.SetActive(true);
            //spellACDText = Instantiate(_cdTextSpellA, spellSlots[2].transform.position, Quaternion.identity);
            TextMeshProUGUI cdText = _cdTextSpellA.GetComponent<TextMeshProUGUI>();
            cdText.text = _activeMage.SpellA.SplRemainingCooldown.ToString();
            ((SpellClicked) spellSlots[2].GetComponent( typeof(SpellClicked))).enabled = false;
        }
        else
        {
            _cdTextSpellA.SetActive(false);
            ((SpellClicked) spellSlots[2].GetComponent( typeof(SpellClicked))).enabled = true;
        }
        if ( _activeMage.SpellB.SplRemainingCooldown > 0)
        {
            _cdTextSpellB.SetActive(true);
            TextMeshProUGUI cdText = _cdTextSpellB.GetComponent<TextMeshProUGUI>();
            cdText.text = _activeMage.SpellB.SplRemainingCooldown.ToString();
            ((SpellClicked) spellSlots[3].GetComponent( typeof(SpellClicked))).enabled = false;
        }
        else
        {
            _cdTextSpellB.SetActive(false);
            ((SpellClicked) spellSlots[3].GetComponent( typeof(SpellClicked))).enabled = true;
        }
    }
    
    public void InitialiseTurnOrder(Mage[] mages)
    {
        _sortOrderHelper = GameObject.Find("Main Camera").GetComponent<SortOrderHelper>();
        _sortOrderHelper.OnUpdate();
        turnPoints = 2;
        _activeMapManager = GameObject.Find("BattleMap").GetComponent<MapManager>();
        _activeMages = mages;
        UnityEngine.Debug.Log("The amount of mages are: " + _activeMages.Length);
        var orderedEnumerable = _activeMages.OrderByDescending(mage => mage.CastSpeed);
        foreach (Mage mage in orderedEnumerable)
        {
            _turnOrder.Add(mage);
        }

        _nextMageToCast = _turnOrder[1];
        _activeMage = _turnOrder[0];
        _currentMageController = _activeMage.GetComponent<MageController>();
        Debug.Log("Active mage = " + _activeMage.name + " Current Spell = " + _activeMage.SpellA);
        _currentMageIndex = 0;
        UpdateSpellBar(_activeMage);
        ResetPlayerInfo();
    }

    private void NextMageTurn()
    {
        _movesRemaining.SetActive(false);
        if (_currentMageIndex < _turnOrder.Count - 1)
        {
            if (_turnOrder[_currentMageIndex] != null)
            {
                if (_turnOrder[_currentMageIndex].IsAlive())
                {
                    _currentMageIndex++;
                    _activeMage = _turnOrder[_currentMageIndex];
                    
                }
                else
                {
                    _turnOrder.Remove(_turnOrder[_currentMageIndex + 1]);
                    NextMageTurn();
                }
            }
        }
        else
        {
            _currentMageIndex = 0;
            _activeMage = _turnOrder[_currentMageIndex];
        }
        _currentMageController = _activeMage.GetComponent<MageController>();
        Debug.Log("Current mage index = " + _currentMageIndex + " Turn order size : " +  _turnOrder.Count);
        UpdateSpellBar(_activeMage);
        ResetPlayerInfo();
        turnPoints = 2;
    }

    public void TakeDamage(GameObject target)
    {
        Vector3 targetPos = target.transform.position; 
        Instantiate(_damageText, new Vector3(targetPos.x, targetPos.y + 1, 0f), Quaternion.identity);
    }

    public void SpellSelected(string spellString)
    {
        switch (spellString)
        {
            case "Move":
                //Start Game logic for move
                Debug.Log("Current mage  = " + _activeMage + " Current Mage Controller = " + _currentMageController + " Current moves left = " + _currentMageController.JumpsLeft);
                if (_jumpsRemaining > 0 && turnPoints > 1)
                {
                    _gizmoManager.MoveSelect(_activeMage);
                    FindObjectOfType<MusicLibrary>().PlaySelectionSound();
                    Cursor.SetCursor(moveCursor, Vector2.zero, CursorMode.ForceSoftware);
                }
                else FindObjectOfType<MusicLibrary>().PlayErrorSound();
                break;
            case "BasicAttack":
                //Start Game logic for basic attack
                clickedSpell = _activeMage.basicSpell;
                Debug.Log("Clicked Spell="+ clickedSpell.name);
                if (turnPoints > 0)
                {
                    turnPoints = 0;
                    InstantiateSelectArrows();
                }
                
                break;
            case "Spell1":
                clickedSpell = _activeMage.SpellA;
                Debug.Log("Clicked Spell="+ clickedSpell.name);
                if (turnPoints > 0)
                {
                    turnPoints = 0;
                    InstantiateSelectArrows();
                }
                break;
            case "Spell2":
                clickedSpell = _activeMage.SpellB;
                if (turnPoints > 0)
                {
                    turnPoints = 0;
                    InstantiateSelectArrows();
                }
                Debug.Log("Clicked Spell="+ clickedSpell.name);
                break;
        }
    }

    public void SelectTarget()
    {
        _currentMageController = _activeMage.GetComponent<MageController>();
        _currentMageController.MagesInRange = magesInRange;
        _currentMageController.StartTargetingState();
    }

    private void InstantiateSelectArrows()
    {
        magesInRange.Clear();
        
        if (_activeMage.CanCast(clickedSpell).Equals("success"))
        {
            if (_gizmoManager.MoveTiles.Count > 0)
            {
                _gizmoManager.ClearTiles();
            }

            int targetTeam = 0;

            foreach (var mage in _activeMages)
            {
                mage.gameObject.GetComponent<SpellTarget>().enabled = false;
                if (clickedSpell.canTargetEnemies)
                {
                    if (_activeMage.Team != mage.Team && MageInRange(mage, clickedSpell))
                    {
                        _selectArrows.Add(Instantiate(_enemySelectIcon,
                            new Vector3(mage.transform.position.x, mage.transform.position.y - 0.2f,
                                mage.transform.position.z), Quaternion.identity));
                        magesInRange.Add(mage);
                        mage.gameObject.GetComponent<SpellTarget>().enabled = true;
                        mage.gameObject.AddComponent<MageClicked>();

                        //SelectTarget();
                    }
                }
                else
                {
                    if (_activeMage.Team == mage.Team && MageInRange(mage, clickedSpell))
                    {
                        _selectArrows.Add(Instantiate(_allySelectIcon,
                            new Vector3(mage.transform.position.x, mage.transform.position.y + 0.2f,
                                mage.transform.position.z), Quaternion.identity));
                        magesInRange.Add(mage);
                        mage.gameObject.AddComponent<MageClicked>();
                        //SelectTarget();
                    }
                }
            }

            if (magesInRange.Count > 0)
            {
                turnPoints = 0;
                _cancelButton.SetActive(true);
                FindObjectOfType<MusicLibrary>().PlaySelectionSound();
                SelectTarget();
                Cursor.SetCursor(spellCastCursor, Vector2.zero, CursorMode.ForceSoftware);
            }
       
        else
            {
                if (GameObject.Find("ToolTipPopUp(Clone)") == null)
                {
                    //GameObject toolTip = Instantiate(fadingToolTip, spellBar.transform.position,Quaternion.identity);
                    spellErrorBox.SetActive(true);
                    spellErrorBox.GetComponentInChildren<TextMeshProUGUI>().text = "No mages in range. \nSelect another spell";
                    //toolTip.transform.SetParent(GameObject.FindWithTag("Canvas").transform);
                    // TextMeshPro toolText = toolTip.GetComponentInChildren<TextMeshPro>();
                    // toolText.text = "No mages in range. \nSelect another spell";
                    FindObjectOfType<MusicLibrary>().PlayErrorSound();
                }
                   
                turnPoints = 2;
            }
        }
            if (_activeMage.CanCast(clickedSpell).Equals("mana"))
            {
                if (GameObject.Find("ToolTipPopUp(Clone)") == null)
                {
                    spellErrorBox.SetActive(true);
                    spellErrorBox.GetComponentInChildren<TextMeshProUGUI>().text = "Insufficient mana. \nSelect another spell";
                    FindObjectOfType<MusicLibrary>().PlayErrorSound();
                }
                turnPoints = 2;
                
            }
            if (_activeMage.CanCast(clickedSpell).Equals("cooldown"))
            {
                if (GameObject.Find("ToolTipPopUp(Clone)") == null)
                {
                    spellErrorBox.SetActive(true);
                    spellErrorBox.GetComponentInChildren<TextMeshProUGUI>().text = "Spell is still on cooldown. \nSelect another spell";
                    FindObjectOfType<MusicLibrary>().PlayErrorSound();
                }
                turnPoints = 2;
                
            }
            _sortOrderHelper.OnUpdate();
        //}
    }

    public bool MageInRange(Mage targetMage, Spell selectedSpell)
    {
        float xDistance = 0;
        float yDistance = 0;
        double blockDistance = 0;
        // xDistance = (int) Math.Abs(_activeMage.BlockPosition.x - targetMage.BlockPosition.x); MOVEMENT DISTANCE CALCULATION
        // yDistance = (int) Math.Abs(_activeMage.BlockPosition.y - targetMage.BlockPosition.y);
        xDistance = (float) Math.Pow(_activeMage.BlockPosition.x - targetMage.BlockPosition.x, 2);// *0.64f;
        yDistance = (float) Math.Pow(_activeMage.BlockPosition.y - targetMage.BlockPosition.y, 2);// * 0.64f;
        blockDistance = (Math.Ceiling(Math.Sqrt(xDistance + yDistance)));
        Debug.Log("Distance between"+ _activeMage.name + " and " + targetMage.name + " is: " + blockDistance + " spell rang is: " + selectedSpell.SpellRange);
        return blockDistance <= selectedSpell.spellRange;
    }

    public void MoveTileSelected(Block selectedBlock)
    {
        Block mageStartingBlock = new Block();
        GameObject activeMageBody = null;
        foreach (GameObject mageBody in _activeMapManager._magesBody)
        {
            if (mageBody.GetComponent<Mage>() == _activeMage)
            {
                activeMageBody = mageBody;
            }
        }
        foreach (Block block in _activeMapManager.MapBlocks)
        {
            if (block.ObjectOnBlock == activeMageBody)
            {
                mageStartingBlock = block;
            }
        }

        _currentMageController = activeMageBody.GetComponent<MageController>();
        
        if (_currentMageController.JumpsLeft > 0 && turnPoints > 1)
        {
            _currentMageController.MoveMageToTile(mageStartingBlock, selectedBlock,_activeMapManager.MapBlocks);
            Rigidbody2D activeIconRB = _selectArrows[0].GetComponent<Rigidbody2D>();
            activeIconRB.MovePosition(new Vector3(selectedBlock.BlockPosition.x, selectedBlock.BlockPosition.y-0.2f, 0));
            //LeanTween.move(_selectArrows[0], new Vector3(selectedBlock.BlockPosition.x, selectedBlock.BlockPosition.y-0.2f, 0), 2.0f).setEase( LeanTweenType.easeOutQuad );
        }
        else
        {
            _gizmoManager.enabled = false;
            moveButton.GetComponent<BoxCollider2D>().enabled = false;
            moveButton.GetComponent<SpellClicked>().enabled = false;
            moveButton.GetComponent<Button>().enabled = false;
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        _jumpsRemaining = _currentMageController.JumpsLeft;
        _sortOrderHelper.OnUpdate();
    }

    private void ResetPlayerInfo()
    {
        _activeMage.SpellA.TickCDDown();
        _activeMage.SpellB.TickCDDown();

        _selectArrows.Add(Instantiate(_activeMageSelectIcon,
            new Vector3(_activeMage.transform.position.x, _activeMage.transform.position.y - 0.2f,
                _activeMage.transform.position.z), Quaternion.identity));

        foreach (Mage mage in _turnOrder)
        {
            mage.gameObject.GetComponent<MageClicked>();
        }

        _jumpsRemaining = 4;
        _currentMageController.JumpsLeft = 4;
        turnPoints = 2;
        if (moveCDText != null)
        {
            Destroy(moveCDText);
        }

        if (spellACDText != null)
        {
            Destroy(spellACDText);
        }
        if (spellBCDText != null)
        {
            Destroy(spellBCDText);
        }
        
        _gizmoManager = GameObject.Find("BattleMap").GetComponent<GizmoManager>();
        _gizmoManager.enabled = true;
        moveButton.GetComponent<BoxCollider2D>().enabled = true;
        moveButton.GetComponent<SpellClicked>().enabled = true;
        moveButton.GetComponent<Button>().enabled = true;
        _sortOrderHelper.OnUpdate();
    }

    public void SpellHitTarget()
    {
        _currentMageController.TransitionToState(_currentMageController.IdleState);
        NextMageTurn();
    }

    public void CancelSpell()
    {
        if (_currentMageController.CurrentState == _currentMageController.FindingTargetState)
        {
            _currentMageController.TransitionToState(_currentMageController.IdleState);
            foreach (var mage in magesInRange)
            {
                mage.gameObject.GetComponent<SpellTarget>().enabled = false;
                Destroy(mage.gameObject.GetComponent<MageClicked>());
                
            }

            for (int i = _selectArrows.Count-1; i > 0; i--)
            {
                Destroy(_selectArrows[i]);
                _selectArrows.RemoveAt(i);
            }
            magesInRange.Clear();
            turnPoints = 2;
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
