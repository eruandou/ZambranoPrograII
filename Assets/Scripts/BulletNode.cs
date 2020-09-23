using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNode
{

    public BulletNode nextNode;
    public Bullet storedBullet;

    public BulletNode (Bullet newBullet)
    {
        storedBullet = newBullet;
    }



}
