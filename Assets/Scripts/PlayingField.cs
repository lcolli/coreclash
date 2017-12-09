using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingField : MonoBehaviour
{

    [Header("Set in Inspector")]
    public float maxHeight;
    public float minHeight, leftAnchor;
    public GameObject dirt, stone, metal, magma, water, treasure, gold, diamond,sand,victory;
    public Zone[] zones;

    [Header("SetDynaimcally")]
    
    int level;
    public bool done = false;
    private float lastLine;


    public void GameStart()
    {
        for(int i=0;i<zones.Length;i++)
        {
            placeZone(i);
            
        }
        Vector3 VictoryLine = new Vector3(7, lastLine - 2);
        Vector3 mirror = new Vector3(-7, lastLine - 2);
        GameObject victoryLeft = Instantiate(victory, this.gameObject.transform);
        GameObject victoryRight = Instantiate(victory, this.gameObject.transform);
        victoryRight.transform.position =VictoryLine ;
        victoryLeft.transform.position = mirror;
        done = true;
    }


    public void placeZone(int zoneNumber)
    {
        List<GameObject> zoneBlocks = new List<GameObject>();

        for (int i=0;i<zones[zoneNumber].numDirt;i++)
        {
            zoneBlocks.Add(dirt);
        }
        for (int i = 0; i < zones[zoneNumber].numStone; i++)
        {
            zoneBlocks.Add(stone);
        }
        for (int i = 0; i < zones[zoneNumber].numMetal; i++)
        {
            zoneBlocks.Add(metal);
        }
        for (int i = 0; i < zones[zoneNumber].numMagma; i++)
        {
            zoneBlocks.Add(magma);
        }
        for (int i = 0; i < zones[zoneNumber].numWater; i++)
        {
            zoneBlocks.Add(water);
        }
        for (int i = 0; i < zones[zoneNumber].numTreasure; i++)
        {
            zoneBlocks.Add(treasure);
        }
        for (int i = 0; i < zones[zoneNumber].numGold; i++)
        {
            zoneBlocks.Add(gold);
        }
        for (int i = 0; i < zones[zoneNumber].numDiamond; i++)
        {
            zoneBlocks.Add(diamond);
        }
        for (int i = 0; i < zones[zoneNumber].numSand; i++)
        {
            zoneBlocks.Add(sand);
        }

        for (int i=0;i<9;i++)
        {
            int rand = Random.Range(0, zoneBlocks.Count-1);
            GameObject toPlace=zoneBlocks[rand];
            if(zoneNumber==0 && i<3 && toPlace.tag=="Treasure")     //treasure can't be on the surface
            {
                i--;
            }
            else
            {
                
                float x, y, z=0;
                x = leftAnchor + (i % 3)*2;
                y = maxHeight - 2 * ((i / 3) + zoneNumber*3);
                Vector3 placement = new Vector3(x, y, z);
                Vector3 mirror = new Vector3(-x, y, z);                
                GameObject blockR=Instantiate(toPlace,this.gameObject.transform);   
                GameObject blockL=Instantiate(toPlace, this.gameObject.transform);
                blockR.transform.position = placement;
                blockL.transform.position = mirror;
                zoneBlocks.RemoveAt(rand);
                lastLine = y;
            }
        }
        
    }
}

[System.Serializable]
public class Zone
{
    public int numDirt, numStone, numMetal, numMagma, numWater, numTreasure, numGold, numDiamond,numSand;
}
