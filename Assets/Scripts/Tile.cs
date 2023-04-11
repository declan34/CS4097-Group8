using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public tile tileObject;

	void Start()
    {
        tileObject = new tile();
    }

    public void changeLocation((int,int) loc)
    {
        tileObject.location = loc;
    }

	public void lockTile()
	{
		tileObject.locked = true;
	}

	public void unlockTile()
	{
		tileObject.locked = false;
	}

}
