using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

    public space[,] Board;

	private static bool firstTurn;

	public static char[] tile_letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
		'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
	public static int[] tile_scores = new int[] { 1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10 };

	public static string[] Dictionary;

	private void Awake()
	{
		Instance = this;
		Board = new space[15, 15];
		for (int i = 0; i < 15; i++)
		{
			for (int j = 0; j < 15; j++)
			{
				Board[i, j] = new space(new Tuple<int, int>(i,j));
			}
		}
		firstTurn = true;
		Dictionary = File.ReadAllLines(".\\Assets\\dictionary.txt");
	}

	public void reset()
	{
		Board = new space[15, 15];
	}

	public void submit()
	{
		Debug.Log("OnSubmit");
		// Check that first turn intersects the middle
		if(firstTurn)
		{
			if (Board[7, 7].letter == ' ')
			{
				Debug.Log("Invalid Move, must use center space");
				return;
			}
			else
			{
				firstTurn = false;
			}
		}

		string word = Instance.FindWord();
		

		if (word == "")
		{
			Debug.Log("No Word Played");
		}
		else
		{
			Debug.Log(word);
			if (Dictionary.Contains(word))
			{
				Debug.Log("VALID WORD");
				int score = ScoreWord(word);
				ScoreSystem.Instance.AddScore(score);
				Debug.Log(score);
			}
			else
			{
				Debug.Log("Not a word, dummy");
			}
			
		}
		
	}
	public string FindWord()
	{
		string word;

		// Validation and Scoring
		for (int i = 0; i < 15; i++)
		{
			for (int j = 0; j < 15; j++)
			{

				//Debug.Log(Board[i, j].CompareTo('\0'));
				if (Board[i, j].letter != ' ') // meaning there is a letter
				{
					//Debug.Log(Board[i, j]);
					
					// Check Horizontal
					if (i + 1 < 15 && Board[i + 1, j].letter != ' ')
					{
						//Debug.Log("Vertical Word");
						word = Board[i, j].ToString() + Board[i + 1, j].ToString();
						int k = 2;

						while (Board[i + k, j].letter != ' ')
						{
							word += Board[i + k, j].ToString();
							k++;
						}

						return word;
					}
					else if (j + 1 < 15 && Board[i, j + 1].letter != ' ')
					{
						//Debug.Log("Horizontal Word");
						word = Board[i, j].ToString() + Board[i, j + 1].ToString();
						int k = 2;

						while (Board[i, j + k].letter != ' ')
						{
							word += Board[i, j + k].ToString();
							k++;
						}
						
						return word;
					}
				}
			}
		}
		return "";
	}
	public int ScoreWord(string word)
	{
		int score = 0;
		int index;
		for (int i = 0; i < word.Length; i++)
		{
			index = System.Array.IndexOf(tile_letters, word[i]);
			score += tile_scores[index];
		}
		return score;
	}
}

public class space
{
	private Tuple<int, int> _location = new Tuple<int, int>(-1, -1);
	private char _letter = ' ';

	public char letter {
		get => _letter;
		set => _letter = value;
	}

	public Tuple<int, int> location
	{
		get => _location;
	}

	public space(Tuple<int,int> loc) 
	{
		_location = loc;
		_letter = ' ';
	}
}
