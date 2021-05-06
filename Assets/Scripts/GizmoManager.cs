using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    private MapManager _activeManager;
    private BattleManager _activeBattleManager;
    private Block[,] _mapBlocks;
    private Vector3 _drawPosition;
    public GameObject moveTile, moveTileHighlighted;
    private List<GameObject> moveTiles = new List<GameObject>();
    [SerializeField] private AudioClip moveClip;
    

    public List<GameObject> MoveTiles => moveTiles;
    private Block _oldMageBlock;

    private void Start()
    {
        _activeManager = GameObject.Find("BattleMap").GetComponent<MapManager>();
        _activeBattleManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
    }

    public void MoveSelect(Mage selectedMage)
    {
        _mapBlocks = _activeManager.MapBlocks;
        for (int y = 0; y < _mapBlocks.GetLength(1); y++)
        {
            for (int x = 0; x < _mapBlocks.GetLength(0); x++)
            {
                int xDistance = (int) Math.Abs(selectedMage.BlockPosition.x - x);
                int yDistance = (int) Math.Abs(selectedMage.BlockPosition.y - y);
                int blockDistance = xDistance + yDistance;
                if (blockDistance < 2 && _mapBlocks[x,y].Walkable == true) //&& _mapBlocks[x,y].Walkable == true
                {
                    _drawPosition =_mapBlocks[x,y].BlockPosition;
                    DrawMoveSquare();
                }            
            }
        }
    }

    public void DrawMoveSquare()
    {
        //Gizmos.color = new Color(169,169,169,0.5f);
        //(_drawPosition,new Vector3(0.64f,0.64f,1f));
        GameObject tile = Instantiate(moveTile, _drawPosition, Quaternion.identity);

        //LeanTween.alpha(tile.GetComponent<RectTransform>(), 0.3f, 0f);
        moveTiles.Add(tile);
    }

    public void MoveToTileClicked(GameObject tileClicked)
    {
        foreach (GameObject moveTile in moveTiles)
        {
            Destroy(moveTile);
        }
        moveTiles.Clear();
        
        Block clickedBlock = new Block();
        foreach (Block block in _mapBlocks)
        {
            if (block.BlockPosition == tileClicked.transform.position)
            {
                clickedBlock = block;
            }
        }
        Debug.Log("Moving to block at :" + clickedBlock.MatrixPosition.x + " , " + clickedBlock.MatrixPosition.y);
        FindObjectOfType<MusicLibrary>().PlayMoveSound();
        _activeBattleManager.MoveTileSelected(clickedBlock);
    }

    public void ClearTiles()
    {
        foreach (GameObject moveTile in moveTiles)
        {
            Destroy(moveTile);
        }
        moveTiles.Clear();
    }
    
    
}
