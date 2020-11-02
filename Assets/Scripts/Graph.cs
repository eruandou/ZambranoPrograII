using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IGraph
{
    void InitializeGraph();
    void AddVertex(int vertex);
    void RemoveVertex(int vertex);
    ISet Vertexes();
    void AddEdge(int id,int vertex1, int vertex2, int value);
    void RemoveEdge(int vertex1, int vertex2);
    bool DoesEdgeExist(int vertex1, int vertex2);
    int EdgeValue(int vertex1, int vertex2);
}

public class Graph : IGraph
{

    static int matrixSize = 100;
    public int[,] adjacencyMatrix;
    public int[] labels;
    public int nodeCount;
    public int[] nodes1;
    public int[] nodes2;

    public Graph()
    {
        InitializeGraph();
    }

    public void InitializeGraph()
    {   //Initialize values
        adjacencyMatrix = new int[matrixSize,matrixSize];
        labels = new int[matrixSize];
        nodeCount = 0;
    }

    public void AddVertex(int vertex)
    {
        //Add label, and set its initial values to 0
        labels[nodeCount] = vertex;
        for (int i = 0; i <= nodeCount; i++)
        {
            adjacencyMatrix[nodeCount, i] = 0;
            adjacencyMatrix[i, nodeCount] = 0;
        }
        
        nodeCount++;
    }

    public void RemoveVertex(int vertexToRemove)
    {
        //Check which vertex we need to remove
        int index = Vertex2Index(vertexToRemove);

        //Reorganize the adjacency matrix based on removed vertex
        for (int j = 0; j < nodeCount; j++)
        {
            adjacencyMatrix[j, index] = adjacencyMatrix[j, nodeCount - 1];
        }

        for (int j = 0; j < nodeCount; j++)
        {
            adjacencyMatrix[index, j] = adjacencyMatrix[nodeCount - 1, j];
        }

        //Reorganize label system
        labels[index] = labels[nodeCount - 1];
        nodeCount--;
    }

    public int Vertex2Index(int vertex)
    {
        int i = nodeCount - 1;
        //based on vertex, check the labels for
        //required vertex to return
        while (i >= 0 && labels[i] != vertex)
        {
            i--;
        }

        return i;
    }

    public ISet Vertexes()
    {
        //create vertexes set for each existing node
        ISet Vertexes = new Set();
    
        for (int i = 0; i < nodeCount; i++)
        {
            Vertexes.Add(labels[i]);
        }
        return Vertexes;
    }

    public void AddEdge(int id,int vertex1, int vertex2, int value)
    {
        //Given two vertexes, adds value to the edge 
        //between them
        int j = Vertex2Index(vertex1);
        int k = Vertex2Index(vertex2);
        adjacencyMatrix[j, k] = value;
    }

    public void RemoveEdge(int vertex1, int vertex2)
    {
        //Given two vertexes, removes the value of
        //the edge between them
        int j = Vertex2Index(vertex1);
        int k = Vertex2Index(vertex2);
        adjacencyMatrix[j, k] = 0;
    }

    public bool DoesEdgeExist(int vertex1, int vertex2)
    {
        //checks if the edge value between two vertexes
        //is not 0.
        int j = Vertex2Index(vertex1);
        int k = Vertex2Index(vertex2);
        return adjacencyMatrix[j, k] != 0;
    }

    public int EdgeValue(int vertex1, int vertex2)
    {
        //Returns edge value between two given
        //vertexes+
        int j = Vertex2Index(vertex1);
        int k = Vertex2Index(vertex2);
        return adjacencyMatrix[j, k];
    }

    public void Dijkstra(int sourceNode)
    {
      (nodes1, nodes2) = AlgDijkstra.Dijkstra(this, sourceNode);
    }
}