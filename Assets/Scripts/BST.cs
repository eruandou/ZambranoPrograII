
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBST
{
    HighscorePlayer Root();
    BSTNode LeftChild();
    BSTNode RightChild();
    bool IsTreeEmpty();
    void InitializeTree();
    void AddElement(ref BSTNode node, HighscorePlayer HSPlayer);
    void DeleteElement(ref BSTNode node, HighscorePlayer HSPlayer);
}







public class BST : IBST
{
    public BSTNode root;

    //Constructor

    public BST()
    {
        InitializeTree();
    }


    public HighscorePlayer Root()
    {
        return root.playerInfo;
    }

    public BSTNode LeftChild()
    {
        return root.leftchild;
    }

    public BSTNode RightChild()
    {
        return root.rightchild;
    }

    public bool IsTreeEmpty()
    {
        return (root == null);
    }

    public void InitializeTree()
    {
        root = null;
    }


    public void AddElement(ref BSTNode node, HighscorePlayer HSPlayer)
    {
        //Check root

        if (node == null)
        {
            node = new BSTNode(HSPlayer);
        }

        //Root's already ocupied, check left and right children
        //to fit new data

        else if (node.playerInfo.Score > HSPlayer.Score)
        {
            Debug.Log("I'm adding to the left branch");
            AddElement(ref node.leftchild, HSPlayer);

        }
        else if (node.playerInfo.Score < HSPlayer.Score)
        {
            AddElement(ref node.rightchild, HSPlayer);
            Debug.Log("I'm adding to the right branch");
        }

    }

    public void DeleteElement(ref BSTNode node, HighscorePlayer HSPlayer)
    {
        if (node != null)
        {
            //if selected node has no children, just make it null

            if (node.playerInfo.Score == HSPlayer.Score && (node.leftchild == null) && (node.rightchild == null))
            {
                node = null;
                Debug.Log($"Deleted node");

            }

            //Reorder left side branch
            else if (node.playerInfo.Score == HSPlayer.Score && node.leftchild != null)
            {
                node.playerInfo = this.Bigger(node.leftchild);
                DeleteElement(ref node.leftchild, node.playerInfo);
                Debug.Log($"Reorder left branch");
            }

            //Reorder right side branch
            else if (node.playerInfo.Score == HSPlayer.Score && node.leftchild == null)
            {
                root.playerInfo = this.Smaller(node.rightchild);
                DeleteElement(ref node.rightchild, node.playerInfo);
                Debug.Log($"Reorder right branch");
            }

            //Desired deleteable node is the right children
            else if (node.playerInfo.Score < HSPlayer.Score)
            {
                DeleteElement(ref node.rightchild, HSPlayer);
                Debug.Log("Check right children");
            }

            //Desired deleteable node is the left children
            else
            {
                DeleteElement(ref node.leftchild, HSPlayer);
                Debug.Log("Check left children");
            }

        }

        Debug.Log("Deleted something from Tree");

    }


    //Returns biggest value (last right children)
    public HighscorePlayer Bigger(BSTNode node)
    {
        if (node.rightchild == null)
        {
            return node.playerInfo;
        }
        else
        {
            return Bigger(node.rightchild);
        }
    }

    //Returns smallest value (last left children)
    public HighscorePlayer Smaller(BSTNode node)
    {
        if (node.leftchild == null)
        {
            return node.playerInfo;
        }
        else
        {
            return Smaller(node.leftchild);
        }
    }


    //Search methods

    public List<HighscorePlayer> InReverseOrder(BSTNode node)
    {
        List<HighscorePlayer> HSPlayers = new List<HighscorePlayer>();

        if (node != null)
        {
            InReverseOrder(node.rightchild);
            HSPlayers.Add(node.playerInfo);
            InReverseOrder(node.leftchild);
        }

        return HSPlayers;

    }




}