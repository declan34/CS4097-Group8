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

	public void submit()
	{

	}
}
