using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MageController : MonoBehaviour //MageController controls the states of the mages and how they affect the mages visually
{
    #region Animator & Mage instantiate and getters

    private Animator _mageAnim;
    private Mage _mage;
    private List<Mage> _activeMages = new List<Mage>();
    private GameObject _mageGameObject;
    private SpriteRenderer _mageRenderer;
    [SerializeField]
    private Sprite _damageSprite;
    public Animator MageAnim => _mageAnim;
    private string[] _animationStates = {"Idle", "Casting", "Moving"};
   
    private List<Mage> magesInRange = new List<Mage>();

    public List<Mage> MagesInRange
    {
        get => magesInRange;
        set => magesInRange = value;
    }
    public Mage Mage => _mage;
    public GameObject MageGameObject => _mageGameObject;
    public SpriteRenderer MageRenderer => _mageRenderer;
    public Sprite DamageSprite => _damageSprite;
    
    

    public string[] AnimationStates => _animationStates;

    #endregion
    
    #region MageStateRegion

    private MageBaseState _currentState;

    public MageBaseState CurrentState => _currentState;
    public readonly MageIdleState IdleState = new MageIdleState();
    public readonly MageCastingState CastingState = new MageCastingState();
    public readonly MageMovingState MovingState = new MageMovingState();
    public readonly MageFindingTargetState FindingTargetState = new MageFindingTargetState();

    #endregion

    #region MoveDetails

    private Block _oldMagePosition;
    private GameObject _newMagePosition;
    private Block nextBlockToMove;
    private Block finalBlock;
    private int jumpsLeft;
    private bool moveCancel = false;
    private GameObject _movesLeftObject;

    public GameObject MovesLeftObject
    {
        get => _movesLeftObject;
        set => _movesLeftObject = value;
    }

    public Block NextBlockToMove
    {
        get => nextBlockToMove;
        set => nextBlockToMove = value;
    }

    public int JumpsLeft
    {
        get => jumpsLeft;
        set => jumpsLeft = value;
    }

    public Block FinalBlock => finalBlock;

    public Block OldMagePosition
    {
        get => _oldMagePosition;
        set => _oldMagePosition = value;
    }
    #endregion

    #region CastingSpell

    private GameObject mageClicked;
    private bool mageHasBeenClicked;
    private bool spellWasCast = false;

    public GameObject MageClicked1
    {
        get => mageClicked;
        set => mageClicked = value;
    }

    public bool MageHasBeenClicked
    {
        get => mageHasBeenClicked;
        set => mageHasBeenClicked = value;
    }

    public bool SpellWasCast
    {
        get => spellWasCast;
        set => spellWasCast = value;
    }
    #endregion
    
    public bool MoveCancel
    {
        get => moveCancel;
        set => moveCancel = value;
    }

    private BattleManager _battleManager;

    public BattleManager BattleManager
    {
        get => _battleManager;
        set => _battleManager = value;
    }

    public Block[,] blockMap;
    void Start()
    {
        _battleManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
        _activeMages = _battleManager.TurnOrder;
        _mageAnim = GetComponentInChildren<Animator>();
        _mageRenderer = GetComponentInChildren<SpriteRenderer>();
        _mage = GetComponent<Mage>();
        TransitionToState(IdleState);
        jumpsLeft = 4;
        _mageGameObject = gameObject;
    }
    void Update()
    {
        _currentState.Update(this);
    }
    public void TransitionToState(MageBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }

    public IEnumerator DelayedTransition(MageBaseState state)
    {
        yield return new WaitForSeconds(1f);
        _currentState = state;
        _currentState.EnterState(this);
    }

    public void TakeDamage()
    {
        
    }
    public IEnumerator PlayerSelectTarget(Spell spell)
    {
        yield return new WaitForSeconds(20f);
    }

    public void MoveMageToTile(Block oldMageBlock, Block selectedTile, Block[,] allBlocks)
    {
        _movesLeftObject = _battleManager.MovesRemaining;
        TransitionToState(MovingState);
        
        _movesLeftObject.SetActive(true);
        
        TextMeshProUGUI movesText = _movesLeftObject.GetComponent<TextMeshProUGUI>();
        movesText.text = jumpsLeft + "";

        Debug.Log("JumpsLeft are: " + jumpsLeft);
        blockMap = allBlocks;
        finalBlock = selectedTile;
        _oldMagePosition = oldMageBlock;
        MovingState.MoveMageToNewSquare(this);
        movesText.text = jumpsLeft + "";
        if (jumpsLeft>0)
        {
            _battleManager.GizmoManager.MoveSelect(Mage);
        }
        
    }
        
       //TransitionToState(IdleState);
        //MovingState.MoveToNextBlock(this);
        public void StartTargetingState()
        {
            TransitionToState(FindingTargetState);
        }
        
        public void MageClicked(GameObject selectedMageBody)
        {
            Mage selectedMage = selectedMageBody.GetComponent<Mage>();
            if (BattleManager.MagesInRange.Contains(selectedMage))
            {
                mageClicked = selectedMageBody;
            }
        }

        public void SpellCast()
        {
            foreach (var arrow in _battleManager.SelectArrows)
            {
                Destroy(arrow);
            }
            _battleManager.SelectArrows.Clear();
            _battleManager.MagesInRange.Clear();
            mageHasBeenClicked = false;
            SpellWasCast = true;
            _battleManager.ActiveMage.CurrentMana -= _battleManager.ClickedSpell.splManaCost;
            if (_battleManager.ActiveMage.CurrentMana < 0)
            {
                _battleManager.ActiveMage.CurrentMana = 0;
            }
        }
}

