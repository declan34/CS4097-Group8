using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

    public char[,] Board;

	private void Awake()
	{
		Instance = this;
		Board = new char[15, 15];
	}

	public void reset()
	{
		Board = new char[15, 15];
	}

	public void submit()
	{
		Debug.Log("OnSubmit");
		Debug.Log(Board);
		// Validation and Scoring
		for(int i = 0; i < 15; i++)
		{
			for(int j = 0; j < 15; j++) 
			{
				if (Board[i, j].CompareTo(null) == 0) // meaning there is a letter
				{
					// Check Horizontal
					if (Board[i+1,j].CompareTo(null) == 0)
					{
						Debug.Log("Horizontal Word");
					}
					else if (Board[i, j+1].CompareTo(null) == 0)
					{
						Debug.Log("Vertical Word");
					}
				}
			}
		}
	}
}
