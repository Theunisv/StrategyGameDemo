using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Debug = UnityEngine.Debug;

public class CanvasControls : MonoBehaviour
{
    public Texture2D cursorArrow;
    public AudioClip[] menuEffects;

    public AudioSource musicPlayer;
    public GameObject btnPlayAi, btnPlayP2, btnExit;

    public GameObject[] selectionPortraits = new GameObject[6]; //magePortrait1, magePortrait2, magePortrait3, magePortrait4, magePortrait5, magePortrait6;
    public Sprite[] mageSprites;
    public Image[] portraitImages = new Image[6];
    public GameObject[] magePortraits;
    public bool mageOneLocked, mageTwoLocked, mageThreeLocked, mageFourLocked, mageFiveLocked, mageSixLocked;
    private int _playerCurrentlyPicking = 0;
    private int _currentPortraitSlot = 0;
    private int _nextPortraitSlot = 1;
    private GameObject _lastSelectedMage;
    public GameObject[] locks = new GameObject[3];
    public List<GameObject> instantiatedPicks = new List<GameObject>();
    public GameObject[] confirmButtons;
    
    private GameObject[] _playerOneSelection = new GameObject[3];
    public GameObject[] _playerTwoSelection = new GameObject[3];
    
    public GameObject selectTitle;
    public GameObject selectionPanelPvP;
    public GameObject mainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        foreach (var mage in magePortraits)
        {
            Destroy(mage.GetComponent<MouseDownTrigger>());
        }
        selectionPanelPvP.SetActive(false);
        instantiatedPicks.Clear();
        /*int index = 0;
        foreach (GameObject portrait in selectionPortraits)
        {
            portraitImages[index] = portrait.GetComponent<Image>();
        }*/
       // Image portrait1 = magePortrait1.GetComponent<Image>();
       // portrait1.image
        // musicPlayer.clip = menuMusic;
        // musicPlayer.Play(0);
    }

    public void PlayVsPlayer()
    {
        foreach (var mage in magePortraits)
        {
            mage.AddComponent<MouseDownTrigger>();
        }
        selectionPanelPvP.SetActive(true);
        mainMenu.SetActive(false);
        confirmButtons[1].SetActive(false);
        
    }
    public void PlayHoverSound()
    {
        musicPlayer.PlayOneShot(menuEffects[0]);
    }
    
    public void PlayClickSound()
    {
        musicPlayer.clip = menuEffects[1];
        musicPlayer.Play(0);
    }

    public void SelectMage(GameObject mage)
    {
        if (_currentPortraitSlot == _nextPortraitSlot)
        {
            _nextPortraitSlot++;
        }
        else
        {
            instantiatedPicks.Remove(_lastSelectedMage);
            Destroy(_lastSelectedMage);   
        }
        Transform portraitPos = selectionPortraits[_currentPortraitSlot].transform;

        _lastSelectedMage = Instantiate(mage, portraitPos.position, Quaternion.identity);
        instantiatedPicks.Add(_lastSelectedMage);
        Debug.Log("Spawning : " + _lastSelectedMage.name);
        _lastSelectedMage.tag = "Untagged";

        switch (_currentPortraitSlot)
        {
            case 0: _playerOneSelection[0] = mage;
                break;
            case 1: _playerOneSelection[1] = mage;
                break;
            case 2: _playerOneSelection[2] = mage;
                break;
            case 3: _playerTwoSelection[0] = mage;
                break;
            case 4: _playerTwoSelection[1] = mage;
                break;
            case 5: _playerTwoSelection[2] = mage;
                break;
        }
       
        
        //portraitImages[_currentPortraitSlot] = Mage.GetComponent<Image>();
        //selectionPortraits[_currentPortraitSlot].GetComponent<UnityEngine.UI.Image>().sprite = Mage.GetComponent<Image>().sprite;
    }
    public void checkIfMagesLocked()
    {
        if (!locks[2].activeInHierarchy)
        {
            SetupPlayer2Picks();
            confirmButtons[1].SetActive(true);
            confirmButtons[0].SetActive(false);
        }
    }

    public void LockMage(int position)
    {
        //Debug.Log("Current position of lock is: " + position + " picking portrait number: " + _currentPortraitSlot);
        if (_playerCurrentlyPicking == 0)
        {
            if (_currentPortraitSlot == position)
            {
                _playerOneSelection[_currentPortraitSlot].SetActive(false);
                _currentPortraitSlot++;
                string lockName = "Lock " + (position + 1);
                GameObject lockToHide = GameObject.Find(lockName);
                lockToHide.SetActive(false);
            }
        }
        else
        {
            if (_currentPortraitSlot == position + 3)
            {
                Debug.Log("Current position of lock is: " + position + " picking portrait number: " + _currentPortraitSlot);
                _playerTwoSelection[_currentPortraitSlot-3].SetActive(false);
                _currentPortraitSlot++;
                string lockName = "Lock " + (position +1);
                GameObject lockToHide = GameObject.Find(lockName);
                lockToHide.SetActive(false);
            }     
        }


    }

    private void SetupPlayer2Picks()
    {
        _playerCurrentlyPicking = 1;
        foreach (var mage in instantiatedPicks)
        {
            if (mage != null)
            {
                mage.SetActive(false);
                
            }
        }
        instantiatedPicks.Clear();
        foreach (var mage in _playerOneSelection)
        {
            mage.SetActive(true);
        }

        TextMeshProUGUI tmpGui = selectTitle.GetComponent<TMPro.TextMeshProUGUI>();
        tmpGui.text = "P2 Select";
        foreach (var lockGO in locks)
        {
            lockGO.SetActive(true);
        }

        Debug.Log("The current pick is number: " + _currentPortraitSlot + " being picked by player: " +
                  _playerCurrentlyPicking);
    }

    public void LoadBattleLevel()
    {
        if (_currentPortraitSlot == 6)
        {
            string savePath = "Assets/Resources/MagesToLoad.txt";

            FileStream fCreate = new FileStream(savePath, FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fCreate);

            string saveInfo = String.Format("{0},{1},{2},{3},{4},{5}",_playerOneSelection[0].name,_playerOneSelection[1].name,_playerOneSelection[2].name,_playerTwoSelection[0].name,_playerTwoSelection[1].name,_playerTwoSelection[2].name);
        
            streamWriter.WriteLine(saveInfo);
            streamWriter.Close();

            
            //AssetDatabase.ImportAsset(savePath); 
            TextAsset asset = Resources.Load("MagesToLoad") as TextAsset;
            SceneManager.LoadScene("BattleScene");
        }
        
    }
}
