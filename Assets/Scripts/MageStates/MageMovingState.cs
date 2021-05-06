using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MageMovingState : MageBaseState
{
    private List<Block> openListBlocks = new List<Block>();
    private List<Block> closedListBlocks = new List<Block>();
    private List<Block> blockPath = new List<Block>();
    public override void EnterState(MageController mageController)
    {
        //throw new System.NotImplementedException();
    }

    public override void Update(MageController mageController)
    {
       // throw new System.NotImplementedException();
    }

    /*public void MoveToNextBlock(MageController mageController)
    {
        Vector2 mageCurrentBlockPos = mageController.Mage.BlockPosition;
        Vector2 finalBlockPosition = mageController.FinalBlock.MatrixPosition;
        Block current = mageController.OldMagePosition;
        int gscore = 0;
        
        var adjacentBlocks = GetWalkableAdjacentBlocks((int) mageCurrentBlockPos.x,(int) mageCurrentBlockPos.y, mageController);
        int hScore = CalculateHScore((int) mageCurrentBlockPos.x, (int) mageCurrentBlockPos.y,
            (int) finalBlockPosition.x, (int) finalBlockPosition.y);
        openListBlocks.Add(current);
        while (openListBlocks.Count > 0)
        {
            var lowest = openListBlocks.Min(b => b.FScore);
            current = openListBlocks.First(b => b.FScore == lowest);
            closedListBlocks.Add(current);
            openListBlocks.Remove(current);
            
            if (closedListBlocks.FirstOrDefault(b => b.HScore == 0 && b.GScore>0)) //MatrixPosition.x == (int) finalBlockPosition.x && (int)b.MatrixPosition.y ==(int) finalBlockPosition.y) != null)
                break;
            
            var adjacentSquares = GetWalkableAdjacentBlocks((int)current.MatrixPosition.x, (int)current.MatrixPosition.y, mageController);
            gscore++;

            foreach(var adjacentSquare in adjacentSquares)
            {
                //Ignore already closed blocks list
                if (closedListBlocks.FirstOrDefault(b => b.MatrixPosition.x == adjacentSquare.MatrixPosition.x
                                                         && b.MatrixPosition.y == adjacentSquare.MatrixPosition.y) != null)
                    continue;
 
                //Check whether block is in openlist
                if (openListBlocks.FirstOrDefault(b => b.MatrixPosition.x == adjacentSquare.MatrixPosition.x
                                                       && b.MatrixPosition.y == adjacentSquare.MatrixPosition.y) == null)
                {
                    // compute its score, set the parent
                    adjacentSquare.GScore = gscore;
                    adjacentSquare.HScore = CalculateHScore((int)adjacentSquare.MatrixPosition.x,
                        (int)adjacentSquare.MatrixPosition.y, (int)finalBlockPosition.x, (int)finalBlockPosition.y);
                    adjacentSquare.FScore = adjacentSquare.GScore + adjacentSquare.HScore;

                    // and add it to the open list
                    openListBlocks.Insert(0, adjacentSquare);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (gscore + adjacentSquare.HScore < adjacentSquare.FScore)
                    {
                        adjacentSquare.GScore = gscore;
                        adjacentSquare.FScore = adjacentSquare.GScore + adjacentSquare.HScore;
                    }
                }
            }
            if (closedListBlocks.Count > 50)
            {
                break;
            }
        }

        MoveMageToNewSquare(mageController);
    }
    
    private int CalculateHScore(int x, int y, int targetX, int targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }
    
    private List<Block> GetWalkableAdjacentBlocks(int x, int y, MageController mageController)
    {
        List<Block> viableBlocks = new List<Block>();
        var proposedBlocks = new List<Vector2>()
        {
            new Vector2(x,y-1),
            new Vector2(x,y+1),
            new Vector2(x-1,y),
            new Vector2(x+1,y)
        };
        foreach (var vector2 in proposedBlocks)
        {
            if ((vector2.x >= 0 && vector2.x < mageController.blockMap.GetLength(0)) &&  (vector2.y >= 0 && vector2.y < mageController.blockMap.GetLength(1))) //Check if x and y is within bounds of map
            {
                if (mageController.blockMap[(int) vector2.x,(int) vector2.y].Walkable)
                {
                    viableBlocks.Add(mageController.blockMap[(int) vector2.x,(int) vector2.y]);
                }
            }
        }

        return viableBlocks;
    }*/

    public void MoveMageToNewSquare(MageController mageController)
    {
        Block targetBlock = mageController.FinalBlock;
        Block currentPos = mageController.OldMagePosition;
        Rigidbody2D mageBody = mageController.gameObject.GetComponent<Rigidbody2D>();

        mageBody.MovePosition(targetBlock.BlockPosition);
        //LeanTween.move(mageController.gameObject, targetBlock.BlockPosition, 2.0f).setEase( LeanTweenType.easeOutQuad );
        
        mageController.Mage.BlockPosition = new Vector2(targetBlock.MatrixPosition.x, targetBlock.MatrixPosition.y);
        targetBlock.WalkedOn(mageController.gameObject);
        currentPos.WalkedOff();
        mageController.OldMagePosition = targetBlock;
        mageController.JumpsLeft--;
        //mageController.TransitionToState(mageController.IdleState);
    }
    
}
