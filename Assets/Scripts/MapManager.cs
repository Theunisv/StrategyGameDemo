using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = System.Object;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
   private Mage[] _player1Mages = new Mage[3];
   private Mage[] _player2Mages = new Mage[3]; 
   [SerializeField]
   private Mage[] _allMages = new Mage[6];
   private Block[,] _mapBlocks;
   private int[,] _intMap;
   public GameObject[] _magesBody = new GameObject[6];
   [SerializeField]
   private GameObject[] magePreFabs;

   [SerializeField] private GameObject[] _destructableObjects;

   private BattleManager _activeBattleManager;

   [SerializeField]
   private List<GameObject> _blockObjects;

   public GameObject mapBackGround;
   private Vector3 initialBackSpawn = new Vector3(-3.426f,9.975f,0);
   #region Getters/Setters

   public Mage[] Mages => _allMages;

   public Block[,] MapBlocks
   {
      get => _mapBlocks;
      set => _mapBlocks = value;
   }

   #endregion
   private int mapXSize, mapYSize;
   private float blockWidth, blockHeight;
   private void Start()
   {
      _activeBattleManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
      GenerateBackGround();
      ReadMageFile();
      mapXSize = 20;
      mapYSize = 15;
      blockWidth = 0.64f;
      blockHeight = 0.64f;
      
      _mapBlocks = new Block[mapXSize, mapYSize];

      for (int yIndex = 0; yIndex < mapYSize; yIndex++)
      {
         for (int xIndex = 0; xIndex < mapXSize; xIndex++)
         {
            Vector3 blockPos = new Vector3(xIndex*blockWidth, yIndex*blockHeight,0f );
            GameObject latestBlock = Instantiate(_blockObjects[0], blockPos, Quaternion.identity);
            Block lastInitBlock = latestBlock.GetComponent<Block>();
            lastInitBlock.BlockName = latestBlock.name;
            lastInitBlock.BlockPosition = blockPos;
            lastInitBlock.Walkable = true;
            lastInitBlock.ObjectOnBlock = null;
            lastInitBlock.MatrixPosition = new Vector2(xIndex,yIndex);
            lastInitBlock.InitialiseBlockScore();
            //Block newBlock = new Block(latestBlock.name, blockPos, true, null);
            //Block blockRealWorld = latestBlock.GetComponent<Block>();
            _mapBlocks[xIndex, yIndex] = lastInitBlock;
         }
      }
      SpawnMages();
      int index = 0;
      for (int i = 0; i < _magesBody.Length; i++)
      {
         _allMages[i] = _magesBody[i].GetComponent<Mage>();
         _allMages[i].InitializeSpells();
         UnityEngine.Debug.Log("Mage Loaded - " + _allMages[i]._name);
      }
      _activeBattleManager.InitialiseTurnOrder(_allMages);
   }
   private void SpawnMages()
   {
      
      int yBottomRange = 0;
      int randomYPosition = 5;
      for (int i = 0; i < 3; i++)
      {
         int xBlockPosition = Random.Range(0, 5); 
         int yBlockPosition = Random.Range(yBottomRange, randomYPosition);
         Block placedBlock = _mapBlocks[xBlockPosition, yBlockPosition];
         GameObject newSpawn = Instantiate(_magesBody[i], placedBlock.BlockPosition, Quaternion.identity);
         placedBlock.WalkedOn(newSpawn); 
         _magesBody[i] = newSpawn;
         _player1Mages[i] = _magesBody[i].GetComponent<Mage>();
         _player1Mages[i].BlockPosition = new Vector2(xBlockPosition, yBlockPosition);
         randomYPosition += 5;
         yBottomRange += 5;
      }

      yBottomRange = 0;
      randomYPosition = 5;
      for (int i = 3; i < 6; i++)
      {
         int xBlockPosition = Random.Range(15, 20); 
         int yBlockPosition = Random.Range(yBottomRange, randomYPosition); 
         Block placedBlock = _mapBlocks[xBlockPosition, yBlockPosition];
         GameObject newSpawn = Instantiate(_magesBody[i], placedBlock.BlockPosition, Quaternion.Euler(0,180,0));
         placedBlock.WalkedOn(newSpawn);
         _magesBody[i] = newSpawn;
         _player2Mages[i-3] = _magesBody[i].GetComponent<Mage>();
         _player2Mages[i - 3].Team = 1;
         _player2Mages[i - 3].BlockPosition = new Vector2(xBlockPosition, yBlockPosition);
         randomYPosition += 5;
         yBottomRange += 5;
      }
      SpawnDestructables();
   }

   private void SpawnDestructables()
   {
      int barrelAmount = Random.Range(3, 5);
      int chestAmount = Random.Range(3, 5);
      int vaseAmount = Random.Range(3, 5);

      foreach (Block currBlock in _mapBlocks)
      {
         if (currBlock.Walkable && currBlock.MatrixPosition.x > 1 && currBlock.MatrixPosition.x < (mapXSize - 1)
             && currBlock.MatrixPosition.y > 1 && currBlock.MatrixPosition.y < (mapYSize - 1) && currBlock.MatrixPosition.x % 3 == 0)
         {
            int spawnChance = Random.Range(0, 15);
            switch (spawnChance)
            {
               case 0:
                  if (barrelAmount > 0)
                  {
                     GameObject barrelObject = Instantiate(_destructableObjects[1], currBlock.BlockPosition,
                        Quaternion.identity);
                     currBlock.WalkedOn(barrelObject);
                  }
                  break;
               case 1:
                  if (chestAmount > 0)
                  {
                     GameObject chestObject = Instantiate(_destructableObjects[0], currBlock.BlockPosition,
                        Quaternion.identity);
                     currBlock.WalkedOn(chestObject);
                  }
                  break;
               case 2:
                  if (vaseAmount > 0)
                  {
                     GameObject vaseObject = Instantiate(_destructableObjects[2], currBlock.BlockPosition,
                        Quaternion.identity);
                     currBlock.WalkedOn(vaseObject);
                  }
                  break;
               default:
                  break;
            }
         }
      }
   }
   private void ReadMageFile()
   {
      string path = "Assets/Resources/MagesToLoad.txt";

      StreamReader reader = new StreamReader(path);
      string fileContent = reader.ReadLine();
      reader.Close();

      string[] magesFromFile =  fileContent.Split(',');
      
      Debug.Log(magesFromFile[0]);
      
      for (int i = 0; i < magesFromFile.Length; i++)
      {
         foreach (var mage in magePreFabs)
         {
            if (mage.name.Contains(magesFromFile[i]))
            {
               _magesBody[i] = mage;
            }
         }
      }
      /*
      _magesBody[0] = Instantiate(Resources.Load(magesFromFile[0])) as GameObject;
      _magesBody[1] = Instantiate(Resources.Load(magesFromFile[1])) as GameObject;
      _magesBody[2] = Instantiate(Resources.Load(magesFromFile[2])) as GameObject;
      _magesBody[3] = Instantiate(Resources.Load(magesFromFile[3])) as GameObject;
      _magesBody[4] = Instantiate(Resources.Load(magesFromFile[4])) as GameObject;
      _magesBody[5] = Instantiate(Resources.Load(magesFromFile[5])) as GameObject;*/
   }

   private void GenerateBackGround()
   {
      int xSize = 13;
      int ySize = 7;
      float blockSize = 1.6f;
      Vector3 initPos;

      for (int y = 0; y <= ySize; y++)
      {
         for (int x = 0; x <= xSize; x++)
         {
            initPos = new Vector3(initialBackSpawn.x +(blockSize* x), initialBackSpawn.y - (blockSize * y), initialBackSpawn.z);
            Instantiate(mapBackGround, initPos, Quaternion.identity);
         }
         
      }
      Object[] prefabs = Resources.LoadAll("Environment/BackgroundBorder",typeof(Object));
      Debug.Log("Total resources loaded: " + prefabs.Length);
      
      foreach (var go in prefabs)
      {
         Instantiate(go as GameObject);
      }
      //Object backgroundBorder : Object[]=Resources.LoadAll("Assets/Music", Object);
   }

   public void OccupyBlock(Block oldPosition, GameObject blockToOccupy)
   {
      foreach (Block currBlock in _mapBlocks)
      {
         
      }
   }
}
