using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{

    public int ID { get; private set; }
    public int MaxEnemies { get; private set; }
    public int TimeLimit { get; private set; }
    public int UnlockLevel1 { get; private set; }
    public int UnlockLevel2 { get; private set; }
    public int Mushroom1Chance { get; private set; }
    public int Mushroom2Chance { get; private set; }
    public int Mushroom3Chance { get; private set; } 
    public int Skeleton1Chance { get; private set; }
    public int Skeleton2Chance { get; private set; }
    public int Skeleton3Chance { get; private set; }
    public int FlyingDemon1Chance { get; private set; }
    public int FlyingDemon2Chance { get; private set; }
    public int FlyingDemon3Chance { get; private set; }



    public LevelData (int id, int maxE, int timeLimit, int level1, int level2, int MChance1, int Mchance2, int Mchance3,int SChance1,int Schance2,int Schance3,int fDChance1,int fDchance2, int fDchance3)
    {
        this.ID = id;
        this.MaxEnemies = maxE;
        this.TimeLimit = timeLimit;
        this.UnlockLevel1 = level1;
        this.UnlockLevel2 = level2;
        this.Mushroom1Chance = MChance1;
        this.Mushroom2Chance = Mchance2;
        this.Mushroom3Chance = Mchance3;
        this.Skeleton1Chance = SChance1;
        this.Skeleton2Chance = Schance2;
        this.Skeleton3Chance = Schance3;
        this.FlyingDemon1Chance = fDChance1;
        this.FlyingDemon2Chance = fDchance2;
        this.FlyingDemon3Chance = fDchance3;
    }









}
