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
	public List<GameObject> computer_hand;
	public int tile_scores_int;
	private int turnCount;
	public static int[] tile_scores;

	public static char[] tile_letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
		'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
	public static int[] tile_scores_default = new int[] { 1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10 };

	private (int, int)[] TRIPLE_WORD_BONUS = new (int, int)[] {(0,0), (0,7), (0,14), (7,0), (7, 14), (14,0), (14, 7), (14,14)};
	private (int, int)[] DOUBLE_WORD_BONUS = new (int, int)[] { (1, 1), (2, 2), (3, 3), (4, 4), (1, 13), (2, 12), (3, 11), (4, 10), (13, 1), (12, 2), (11, 3), (10, 4), (13, 13), (12, 12), (11, 11), (10, 10) };
	private (int, int)[] TRIPLE_LETTER_BONUS = new (int, int)[] { (1, 5), (1, 9), (5, 1), (5, 5), (5, 9), (5, 13), (9, 1), (9, 5), (9, 9), (9, 13), (13, 5), (13, 9)};
	private (int, int)[] DOUBLE_LETTER_BONUS = new (int, int)[] { (2, 6), (2, 8), (3, 7), (6, 2), (6, 6), (6, 8), (6, 12), (7, 3), (7, 11), (8, 2), (8, 6), (8, 8), (8, 12), (11, 7), (12, 6), (12, 8)};
	private (int, int)[] ALL_BONUS;
	
	public static int[] tile_scores_1s = new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
	public static int[] tile_scores_max = new int[] {3, 3, 3, 3, 3, 3, 3, 3, 3, 8, 8, 3, 3, 3, 3, 3, 30, 3, 3, 3, 3, 3, 8, 8, 8, 8, 30};
	public static int[] tile_scores_random = new int[] { 2, 3, 1, 3, 2, 2, 3, 4, 1, 8, 5, 1, 3, 2, 1, 3, 20, 1, 3, 2, 1, 8, 8, 8, 4, 20};

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
		System.Random rng = new System.Random(); 
		tile_scores_int = rng.Next(4);
		if (tile_scores_int == 0)
			tile_scores = tile_scores_default;
		else if (tile_scores_int == 1)
			tile_scores = tile_scores_1s;
		else if (tile_scores_int == 2)
			tile_scores = tile_scores_max;
		else if (tile_scores_int == 3)
			tile_scores = tile_scores_random;

		ALL_BONUS = TRIPLE_WORD_BONUS.Concat(DOUBLE_WORD_BONUS).ToArray().Concat(TRIPLE_LETTER_BONUS).ToArray().Concat(DOUBLE_LETTER_BONUS).ToArray();
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
				List<int> score_mods = new List<int>();
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
					(int firstPlayedTileX, int firstPlayedTileY) = played_tiles[0].GetComponent<Tile>().getLocation();
					while (firstPlayedTileY > 0 && Board[firstPlayedTileX, firstPlayedTileY-1].letter != ' ')
					{
						firstPlayedTileY--;
					}
					Debug.Log($"{firstPlayedTileX}, {firstPlayedTileY}");
					playedWord = getWordHorizontal(firstPlayedTileX, firstPlayedTileY);
					Debug.Log($"Played Word {playedWord}");
					
				}
				else
				{
					played_tiles = played_tiles.OrderBy(t => t.GetComponent<Tile>().tileObject.location.Item1).ToList();
					(int firstPlayedTileX, int firstPlayedTileY) = played_tiles[0].GetComponent<Tile>().getLocation();
					while (firstPlayedTileX > 0 && Board[firstPlayedTileX-1, firstPlayedTileY].letter != ' ')
					{
						firstPlayedTileX--;
					}
					Debug.Log($"{firstPlayedTileX}, {firstPlayedTileY}");
					playedWord = getWordVertical(firstPlayedTileX, firstPlayedTileY);
					Debug.Log($"Played Word {playedWord}");

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
			turnCount++;
		}
		computerPlay();
		turnCount++;

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

	public void computerPlay()
	{
		bool solution = false;
		for (int row = 0; row < 15; row++)
		{
			for (int col = 0; col < 15; col++)
			{
				if (Board[row, col].letter == ' ')
				{
					(string, string) word = FindValidWord(row, col);
					if(word.Item2 == "CENTER")
					{
						Debug.Log("CENTER");
						//for(int i = 0; i < computer_hand.Count; i++)
						//{
						//	if (word.Item1 == computer_hand[i].name)
						//	{
						//		//move tile to the location
						//	}
						//}
						return;
					}
                    if (word.Item2 == "LEFT")
                    {
                        Debug.Log("LEFT");
						return;
                    }
                    if (word.Item2 == "ABOVE")
                    {
                        Debug.Log("ABOVE");
						return;
                    }
                    if (word.Item2 == "RIGHT")
                    {
                        Debug.Log("RIGHT");
						return;
                    }
                    if (word.Item2 == "BELOW")
                    {
                        Debug.Log("BELOW");
						return;
                    }
                }
			}
		}
        Debug.Log("REDRAW");
		DrawScript.Instance.computerDraw();
		return;
    }

	public (string, string) FindValidWord(int row, int col)
	{

		//check if the word connects to existing tile
		if ((col + 1 < 15 && Board[row, col + 1].letter != ' ') || (col - 1 >= 0 && Board[row, col - 1].letter != ' ') ||
			(row + 1 < 15 && Board[row + 1, col].letter != ' ') || (row - 1 >= 0 && Board[row - 1, col].letter != ' '))
		{
			return ("", "NONE");
		}

		//if there is letters above and below the space
		if ((row + 1 < 15 && Board[row + 1, col].letter != ' ') && (row - 1 >= 0 && Board[row - 1, col].letter != ' '))
		{
			//get the letters above the space
			int k = row;
			while (row - 1 >= 0 && Board[row - 1, col].letter != ' ')
			{
				k -= 1;
			}
			string word1 = getWordVertical(k, col);

			//get the letters below the space
			string word2 = getWordVertical(row + 1, col);

			//find a letter to connect the two "words" together
			for (int i = 0; i < 7; i++)
			{
				string totalword = word1 + computer_hand[i].name[0] + word2;
				foreach (string word in Dictionary)
				{
					if (totalword == word)
					{
						return (computer_hand[i].name, "CENTER");
					}
				}
			}
		}
		else
		{
			//if there is letter above
			if (row - 1 >= 0 && Board[row - 1, col].letter != ' ')
			{
				//get the letters above the space
				int k = row;
				while (row - 1 >= 0 && Board[row - 1, col].letter != ' ')
				{
					k -= 1;
				}
				string word1 = getWordVertical(k, col);

				//find the possible word combinations added to the end of word1
				List<List<GameObject>> permutations = Permute(computer_hand);
				foreach (string word in Dictionary)
				{
					string word2 = "";
					foreach(List<GameObject> g in permutations)
					{
						foreach(GameObject go in g)
						{
                            word2 += go.name[0];
                        }
						if(word1 + word2 == word)
						{ 
							return (word2, "BELOW"); 
						}
					}
				}
			}
			//if there is letter below
			if (row + 1 < 15 && Board[row + 1, col].letter != ' ')
			{
				//get word below the space
				string word2 = getWordVertical(row + 1, col);

                //find the possible word combinations added to the begining of word2
                List<List<GameObject>> permutations = Permute(computer_hand);
                foreach (string word in Dictionary)
                {
                    string word1 = "";
                    foreach (List<GameObject> g in permutations)
                    {
                        foreach (GameObject go in g)
                        {
                            word1 += go.name[0];
                        }
                        if (word1 + word2 == word)
                        {
                            return (word1, "ABOVE");
                        }
                    }
                }
            }
		}
		//if there is letter to left and right
		if ((col + 1 < 15 && Board[row, col + 1].letter != ' ') && (col - 1 >= 0 && Board[row, col - 1].letter != ' '))
		{
            //get the letters left of the space
            int k = col;
            while (col - 1 >= 0 && Board[row, col - 1].letter != ' ')
            {
                k -= 1;
            }
            string word1 = getWordHorizontal(row, k);

            //get the letters right of the space
            string word2 = getWordHorizontal(row, col + 1);

            //find a letter to connect the two "words" together
            for (int i = 0; i < 7; i++)
            {
                string totalword = word1 + computer_hand[i].name[0] + word2;
                foreach (string word in Dictionary)
                {
                    if (totalword == word)
                    {
                        return (computer_hand[i].name, "CENTER");
                    }
                }
            }
        }
		else
		{
			//if there is letter to left
			if (col - 1 >= 0 && Board[row, col - 1].letter != ' ')
			{
                //get the letters left of the space
                int k = col;
                while (col - 1 >= 0 && Board[row, col - 1].letter != ' ')
                {
                    k -= 1;
                }
                string word1 = getWordHorizontal(row, k);

                //find the possible word combinations added to the end of word1
                List<List<GameObject>> permutations = Permute(computer_hand);
                foreach (string word in Dictionary)
                {
                    string word2 = "";
                    foreach (List<GameObject> g in permutations)
                    {
                        foreach (GameObject go in g)
                        {
                            word2 += go.name[0];
                        }
                        if (word1 + word2 == word)
                        {
                            return (word2, "RIGHT");
                        }
                    }
                }
            }
			//if there is letter to right
			if (col + 1 < 15 && Board[row, col + 1].letter != ' ')
			{
                //get word right of the space
                string word2 = getWordHorizontal(row, col + 1);

                //find the possible word combinations added to the begining of word2
                List<List<GameObject>> permutations = Permute(computer_hand);
                foreach (string word in Dictionary)
                {
                    string word1 = "";
                    foreach (List<GameObject> g in permutations)
                    {
                        foreach (GameObject go in g)
                        {
                            word1 += go.name[0];
                        }
                        if (word1 + word2 == word)
                        {
                            return (word1, "LEFT");
                        }
                    }
                }
            }
		}
		return ("", "NONE");
	}

    List<List<GameObject>> Permute(List<GameObject> list)
    {
        // Initialize variables
        List<List<GameObject>> permutations = new List<List<GameObject>>();

        // Call the recursive helper function
        Permute(list, 0, list.Count - 1, permutations);

        // Return the list of permutations
        return permutations;
    }

    void Permute(List<GameObject> list, int start, int end, List<List<GameObject>> permutations)
    {
        // Check if we have reached the end of the list
        if (start == end)
        {
            // Add the current permutation to the list
            permutations.Add(new List<GameObject>(list));
        }
        else
        {
            // Loop through each element in the list
            for (int i = start; i <= end; i++)
            {
                // Swap the current element with the start element
                Swap(list, start, i);

                // Recursively permute the rest of the list
                Permute(list, start + 1, end, permutations);

                // Swap back the elements to restore the original order
                Swap(list, start, i);
            }
        }
		return;
    }

    void Swap(List<GameObject> list, int i, int j)
	{
        GameObject temp = list[i];
        list[i] = list[j];
        list[j] = temp;
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