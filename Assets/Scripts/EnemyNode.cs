using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNode
{

    public EnemyNode NextNode { get; set; }

    public Enemy EnemyData { get; set; }


    public EnemyNode(Enemy newEnemy)
    {
        this.EnemyData = newEnemy;
        this.NextNode = null;
    }










}
