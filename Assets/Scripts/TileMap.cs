/*
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Debug;

public class TileMap : MonoBehaviour
{
    [SerializeField]
    private InnerTileType[] _tileTypes; //an array of tiletypes used to evaluate how players are intended to use each tile.

    private String[,] _mapTiles;
    private Vector3[,] _tileRealWorldPositions;
    private Vector3Int[] _allPositions;
    
    private int[,] _tiles;

    private int mapWidth = 18;
    private int mapHeight = 15;

    [SerializeField]
    private Tilemap activeMap;

    private TileBase[] activeTiles;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        //activeMap = (Tilemap)GameObject.Find("Tilemap").;
        //allocate the size of the multi-dime array 
        _tiles = new int[mapWidth, mapHeight];
        
        //set each value of tiles to 0, meaning tile is empty
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                _tiles[x,y]= 0;
            }
            
        }

        PopulateTileMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            //Select single tile and retrieve information.
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mousePosTranslated = activeMap.WorldToCell(mousePos);
            Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {0}]", pos.x, pos.y));
            TileBase tile = activeMap.GetTile<Tile>(mousePosTranslated);

            Debug.Log(string.Format("Tile is: {0}", tile.name));
            Debug.Log("The bounds of the tilemap is :" + activeMap.localBounds);
        }

    }

    private void PopulateTileMap()
    {
        BoundsInt mapBounds = activeMap.cellBounds;
        int tileSize = 0;
        int index = 0;
        _allPositions = new Vector3Int[337];
        _tileRealWorldPositions = new Vector3[mapBounds.size.x,mapBounds.size.y];
        
        foreach (var position in activeMap.cellBounds.allPositionsWithin)
        { 
            _allPositions[index] = position;
            index++;
        }

        activeTiles = activeMap.GetTilesBlock(mapBounds);
        _mapTiles = new string[mapBounds.size.x,mapBounds.size.y];

        index = 0;
        
        for (int x = 0; x < mapBounds.size.x; x++)
        {
            for (int y = 0; y < mapBounds.size.y; y++)
            {
                TileBase tile = activeTiles[x + y * mapBounds.size.x];
                if (activeTiles[x + y * mapBounds.size.x].name != null)
                {
                    _mapTiles[x, y] = activeTiles[x + y * mapBounds.size.x].name;
                   _tileRealWorldPositions[x, y] = activeMap.CellToWorld(_allPositions[index]);
                    Debug.Log("Tile: " + tile.name+ " added to array at position: "+ x + ", " +y);
                    
                }
            }
        }

        
       // Debug.Log("Total tiles = " + totalTiles);
        /*_mapTiles = new Tile[mapWidth, mapHeight];
        
                //set each value of tiles to 0, meaning tile is empty
                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        _mapTiles[x,y]= null;
                    }
                    
                }
#1#
        
    }



}
*/
