using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{

    public Graph levelsGraph;


    private Dictionary<int, LevelNode> vertexDictionary = new Dictionary<int, LevelNode>();
    private Dictionary<int, Obj_Edge> edgeDictionary = new Dictionary<int, Obj_Edge>();

    private const string NODE_NAME_CONVENTION = "levelNode";
  
    private const int NODES_AMOUNT = 11;

    private const string UNLOCKED_LEVELS_PATH = "/unlockedLevels.json";

    int[] vertexOrigin = { 1, 2, 3, 4, 5, 4, 7, 8, 9, 9 };
    int[] vertexDestination = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

    private LevelNode originNode;
    private LevelNode destinyNode;

    private Ship ship; 


    private void Start()
    {
        ship = FindObjectOfType<Ship>();

        //Adds new graph
        levelsGraph = new Graph();

        //Adds vertexes from array
        for (int i = 0; i < NODES_AMOUNT; i++)
        {            
            //Get node from currentScene
            LevelNode lvlNode = GameObject.Find($"{NODE_NAME_CONVENTION}{i + 1}").GetComponent<LevelNode>();
            //Add it to dictionary (quick reference by id)
            vertexDictionary.Add(lvlNode.id, lvlNode);
            //Add it to Graph system
            levelsGraph.AddVertex(lvlNode.id);            
        }

        //Adds the edge values for different vertexes both ways
        for (int i = 0; i < vertexOrigin.Length; i++)
        {
            //Add edge for one way around
            Obj_Edge newEdge1 = new Obj_Edge();
            newEdge1.id = i;
            newEdge1.idOrig = vertexOrigin[i];
            newEdge1.idDest = vertexDestination[i];
            newEdge1.value = 1;
            edgeDictionary.Add(newEdge1.id, newEdge1);
            levelsGraph.AddEdge(newEdge1.id, vertexOrigin[i], vertexDestination[i], 1);           

            //Add edge for the other way 
            Obj_Edge newEdge2 = new Obj_Edge();
            newEdge2.id = i + NODES_AMOUNT + 1;
            newEdge2.idOrig = vertexDestination[i];
            newEdge2.idDest = vertexOrigin[i];
            newEdge2.value = 1;
            edgeDictionary.Add(newEdge2.id, newEdge2);
            levelsGraph.AddEdge(newEdge2.id, vertexDestination[i], vertexOrigin[i], 1);
        }

        originNode = vertexDictionary[Gamemanager.instance.lastAccessedLevel];

        ship.transform.position = originNode.transform.position;

        CheckUnlockState();

    }

    public LevelNode WhatIsDestinyNode()
    {
        return destinyNode;
    }

    public void SelectDestination(LevelNode selectedLevelNode)
    {
        if (destinyNode != null)
        {
            originNode = destinyNode;
        }

        destinyNode = selectedLevelNode;

        //Calculate roads

        levelsGraph.Dijkstra(originNode.id);

        List<Vector2> roadConnection = new List<Vector2>();

        roadConnection.Insert(0, destinyNode.transform.position);

        int aux = destinyNode.id;


        while (levelsGraph.nodes1[aux - 1] != originNode.id)
        {
            roadConnection.Insert(0, vertexDictionary[levelsGraph.nodes1[aux - 1]].transform.position);
            aux = levelsGraph.nodes1[aux - 1];
        }

        roadConnection.Insert(0, originNode.transform.position);

        //Disable all selections
        for (int i = 1; i <= NODES_AMOUNT; i++)
        {
            if (vertexDictionary[i].id != originNode.id && vertexDictionary[i].id != destinyNode.id)
            {
                vertexDictionary[i].ChangeSelectableness(false);
            }
        }

        ship.GetNewRoad(roadConnection);
    }

    public bool CanSelectNewNode()
    {
        return !ship.ShipIsMoving;
    }

    public void ReachedDestination()
    {
        for (int i = 1; i <= NODES_AMOUNT; i++)
        {
            if (vertexDictionary[i].id != destinyNode.id && vertexDictionary [i].IsUnlocked())
            {
                vertexDictionary[i].ChangeSelectableness(true); 
            }
        }
    }

    private void CheckUnlockState()
    {
        (bool [] levels,bool [] isComplete) = LevelsSaver.Load();

        for (int i = 1; i <= NODES_AMOUNT; i++)
        {
            vertexDictionary[i].UnlockLevel(levels[i - 1]);
            vertexDictionary[i].ChangeCompleteness(isComplete[i - 1]);
        }
    }
}
