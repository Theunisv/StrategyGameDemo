using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private string _blockName;
    private Vector3 _blockPosition;
    private bool _walkable;
    private GameObject _objectOnBlock;
    private Vector2 _matrixPosition;
    private int fScore, gScore, hScore;

    #region Getters & Setters

    public int FScore
    {
        get => fScore;
        set => fScore = value;
    }

    public int GScore
    {
        get => gScore;
        set => gScore = value;
    }

    public int HScore
    {
        get => hScore;
        set => hScore = value;
    }
    public string BlockName
    {
        get => _blockName;
        set => _blockName = value;
    }
    public Vector3 BlockPosition
    {
        get => _blockPosition;
        set => _blockPosition = value;
    }

    public GameObject ObjectOnBlock
    {
        get => _objectOnBlock;
        set => _objectOnBlock = value;
    }

    public bool Walkable
    {
        get => _walkable;
        set => _walkable = value;
    }

    public Vector2 MatrixPosition
    {
        get => _matrixPosition;
        set => _matrixPosition = value;
    }
    #endregion

    public Block()
    {
    }

    public Block(string blockName, Vector3 blockPosition, bool walkable, Vector2 matrixPosition,
        GameObject objectOnBlock = null)
    {
        _blockName = blockName;
        _blockPosition = blockPosition;
        _walkable = walkable;
        _objectOnBlock = objectOnBlock;
        _matrixPosition = matrixPosition;
    }

    public void WalkedOn(GameObject occupyingObject)
    {
        _walkable = false;
        _objectOnBlock = occupyingObject;
    }
    public void WalkedOff()
    {
        _walkable = true;
        _objectOnBlock = null;
    }

    public void InitialiseBlockScore()
    {
        fScore = 0;
        gScore = 0;
        hScore = 0;
    }
}
