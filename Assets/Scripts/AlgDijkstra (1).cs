﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public static class AlgDijkstra
    {
    public static int[] distance;
    public static string[] nodos;
    public static int[] nodos1;
    public static int[] nodos2;

        private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                // obtengo siempre el nodo con la menor distancia calculada
                // solo lo verifico en los nodos que no tienen seteado ya un camino (shortestPathTreeSet[v] == false)
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            // devuelvo el nodo calculado
            return minIndex;
        }

        public static (int [], int []) Dijkstra(Graph initGraph, int source)
        {
            // obtengo la matriz de adyacencia del TDA_Grafo
            int[,] graph = initGraph.adjacencyMatrix;
            
            // obtengo la cantidad de nodos del TDA_Grafo
            int verticesCount = initGraph.nodeCount;

            // obtengo el indice del nodo elegido como origen a partir de su valor
            source = initGraph.Vertex2Index(source);

            // vector donde se van a guardar los resultados de las distancias entre 
            // el origen y cada vertice del grafo
            distance = new int[verticesCount];

            bool[] shortestPathTreeSet = new bool[verticesCount];

             nodos1 = new int[verticesCount];
             nodos2 = new int[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                // asigno un valor maximo (inalcanzable) como distancia a cada nodo
                // cualquier camino que se encuentre va a ser menor a ese valor
                // si no se encuentra un camino, este valor maximo permanece y es el 
                // indica que no hay ningun camino entre el origen y ese nodo
                distance[i] = int.MaxValue;

                // seteo en falso al vector que guarda la booleana cuando se encuentra un camino
                shortestPathTreeSet[i] = false;

                nodos1[i] = nodos2[i] = -1;
            }

            // la distancia al nodo origen es 0
            distance[source] = 0;
            nodos1[source] = nodos2[source] = initGraph.labels[source];

            // recorro todos los nodos (vertices)
            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                // recorro todos los nodos (vertices)
                for (int v = 0; v < verticesCount; ++v)
                {
                    //Convert to boolean will return true if int !=0, hence why it's used.

                    // comparo cada nodo (que aun no se haya calculado) contra el que se encontro que tiene la menor distancia al origen elegido
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                    {
                        // si encontré una distancia menor a la que tenia, la reasigno la nodo
                        distance[v] = distance[u] + graph[u, v];
                        // guardo los nodos para reconstruir el camino
                        nodos1[v] = initGraph.labels[u];
                        nodos2[v] = initGraph.labels[v];
                    }   
                }

        }
        return (nodos1, nodos2);
    }
}

