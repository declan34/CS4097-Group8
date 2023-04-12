using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

    public space[,] Board;
	public List<GameObject> tiles_on_board;
	public List<GameObject> tiles_on_rack;

	private int turnCount;

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
				Board[i, j] = new space((i,j));
			}
		}
		turnCount = 0;
		Dictionary = File.ReadAllLines(".\\Assets\\dictionary.txt");
	}

	public void reset()
	{
		Board = new space[15, 15];
	}

	public void submit()
	{
		//users turns are the first turn and every-other turn a.k.a. mod 2
		if (turnCount % 2 == 0)
		{
			Debug.Log("OnSubmit");
			// Check that first turn intersects the middle
			if (turnCount == 0)
			{
				if (Board[7, 7].letter == ' ')
				{
					Debug.Log("Invalid Move, must use center space");
					return;
				}
			}

			if( !ValidateBoard() )
			{
				Debug.Log("BOARD NOT VALID");
				return;
			}
			else
			{
				List<GameObject> played_tiles = new List<GameObject>();
				bool horizontalWord = true;
				bool verticalWord = true;
				string playedWord;
				for (int i = 0; i < tiles_on_rack.Count; i++)
				{
					if (tiles_on_rack[i].GetComponent<Tile>().tileObject.location != (-1, -1))
					{
						played_tiles.Add(tiles_on_rack[i]);
						tiles_on_rack[i].GetComponent<Tile>().lockTile();
						tiles_on_board.Add(tiles_on_rack[i]);
						tiles_on_rack.RemoveAt(i);
						i--; // Because we removed an element, we need to subtract one from i
					}
				}
				
				// Played words should always be in a single row or column
				for (int j = 0; j < played_tiles.Count - 1; j++)
				{
					(int currTileX, int currTileY) = played_tiles[j].GetComponent<Tile>().tileObject.location;
					(int nextTileX, int nextTileY) = played_tiles[j + 1].GetComponent<Tile>().tileObject.location;

					if (currTileX != nextTileX)
						horizontalWord = false;
					if (currTileY != nextTileY)
						verticalWord = false;
				}
				if (!horizontalWord && !verticalWord)
				{
					Debug.Log("INVALID PLAY, Not all played tiles are in a line");
					return;
				}
				else if (horizontalWord)
				{
					played_tiles = played_tiles.OrderBy(t => t.GetComponent<Tile>().tileObject.location.Item2).ToList();
					playedWord = getWordHorizontal(played_tiles[0].GetComponent<Tile>().tileObject.location.Item1, played_tiles[0].GetComponent<Tile>().tileObject.location.Item2);
				}
				else
				{
					played_tiles = played_tiles.OrderBy(t => t.GetComponent<Tile>().tileObject.location.Item1).ToList();
					playedWord = getWordVertical(played_tiles[0].GetComponent<Tile>().tileObject.location.Item1, played_tiles[0].GetComponent<Tile>().tileObject.location.Item2);
				}
				ScoreSystem.Instance.AddPlayerScore(ScoreWord(playedWord));
			}
			/*
			Debug.Log("Tiles on Board:");
            foreach(GameObject obj in tiles_on_board)
			{
				Debug.Log(obj.name);
			}
			*/

			RefillRack();
		}
        return;
	}

	public bool ValidateBoard()
	{
		for (int row = 0; row < 15; row++)
		{
			for (int col = 0; col < 15; col++)
			{
				if (Board[row, col].letter != ' ') //if there is a letter at the position
				{
					if( CheckHorizontal(row, col) ) //check if word starts here horizontally
					{
						string word = getWordHorizontal(row, col);
						if (!Dictionary.Contains(word))
						{ //checks word
                            Debug.Log(word + " is not a word");
                            return false;
						}
						Debug.Log("horizontal word: " + word);
					}

					if( CheckVertical(row, col)) //if the word starts here vertically
					{
                        string word = getWordVertical(row, col);
                        if (!Dictionary.Contains(word))
                        { //checks word
							Debug.Log(word + " is not a word");
                            return false;
                        }
						Debug.Log("vertical word: " + word);
                    }
				}
			}
		}

		return true;
    }

	public bool CheckHorizontal(int row, int col) 
	{ //checks to see if the tile is the start of a word vertically
		if ( (col +1) < 15) //range check
		{
			if (Board[row, col+1].letter != ' ') //if there is a letter below the targeted tile
			{
				if (Board[row, col - 1].letter != ' ') //if there is a letter above and below target tile
				{
					return false; //return false because the tile is in the middle of a word
				}
				else
				{
                    return true; //else returns true because the tile is at the start of a word
                }
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

    public bool CheckVertical(int row, int col)
    { //checks to see if the tile is the start of a word horizontally
        if ((row + 1) < 15)
        {
            if (Board[row + 1, col].letter != ' ') //if there is a letter to the right of the targeted tile
            {
                if (Board[row - 1, col].letter != ' ') //if there is a letter left and right of the target tile
                {
                    return false; //return false because the tile is in the middle of a word
                }
                else
				{
                    return true; //else returns true because the tile is at the start of a word
                }
            }
			else
			{
				return false;
			}
        }
		else
		{
			return false;
		}
    }

	public string getWordVertical(int row, int col)
	{
        int k = row;
        string word = "";
        while (k < 15 && Board[k, col].letter != ' ') //while on the board and reading letters
        { //collects the word and locks the tiles
            word += Board[k, col].letter.ToString();
            k++;
        }
		return word;
    }

	public string getWordHorizontal(int row, int col)
	{
        int k = col;
        string word = "";
        while (k < 15 && Board[row, k].letter != ' ') //while on the board and reading letters
        { //collects the word
            word += Board[row, k].letter.ToString();
			k++;
        }
		return word;
    }

	public int ScoreBoard()
    { //to be used after board is validated, adds the values of all words together
		int score = 0;
        for (int row = 0; row < 15; row++)
        {
            for (int col = 0; col < 15; col++)
            {
                if (Board[row, col].letter != ' ') //if there is a letter at the position
                {
                    if (CheckHorizontal(row, col)) //check if word starts here horizontally
                    {
                        string word = getWordHorizontal(row, col);
						score += ScoreWord(word);
                    }

                    if (CheckVertical(row, col)) //if the word starts here vertically
                    {
                        string word = getWordVertical(row, col);
                        score += ScoreWord(word);
                    }
                }
            }
        }

		return score;
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

	public void RefillRack()
	{
		for(int i = 0; i < 7; i++)
		{
			//if ( /* tile_on_rack[i] is on the board */ )
			//{
				//tiles_on_rack[i].GetComponent<Tile>().lockTile(); //lock the tile
				//DrawScript.Instance.FillRack(i); //fill the rack where the tile used to be
			//}
        }
		return;
    }
}

public class space
{
	private (int,int) _location;
	private char _letter = ' ';

	public char letter
	{
		get => _letter;
		set => _letter = value;
	}

	public (int,int) location
	{
		get => _location;
	}

	public space((int,int) loc)
	{
		_location = loc;
		_letter = ' ';
	}
}

public class tile
{
	private (int, int) _location;
	private bool _locked;

	public (int, int) location
	{
		get => _location;
		set => _location = value;
	}

	public bool locked
	{
		get => _locked;
		set => _locked = value;
	}

	public tile()
	{
		_locked = false;
		_location = (-1, -1);
	}
}