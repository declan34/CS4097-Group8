using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateSprite : MonoBehaviour
{
	public Sprite[] tiles;
    public Sprite[] tiles_1s;
	private Sprite tile_letter;
    private Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
		imageComponent = GetComponent<Image>();
		int index = System.Array.IndexOf(GameManager.tile_letters, name[0]);
        switch (name[0])
        {
            case 'A':
                index = 0;
                break;
            case 'B':
                index = 1;
                break;
            case 'C':
                index = 2;
                break;
            case 'D':
                index = 3;
                break;
            case 'E':
                index = 4;
                break;
            case 'F':
                index = 5;
                break;
            case 'G':
                index = 6;
                break;
            case 'H':
                index = 7;
                break;
            case 'I':
                index = 8;
                break;
            case 'J':
                index = 9;
                break;
            case 'K':
                index = 10;
                break;
            case 'L':
                index = 11;
                break;
            case 'M':
                index = 12;
                break;
            case 'N':
                index = 13;
                break;
            case 'O':
                index = 14;
                break;
            case 'P':
                index = 15;
                break;
            case 'Q':
                index = 16;
                break;
            case 'R':
                index = 17;
                break;
            case 'S':
                index = 18;
                break;
            case 'T':
                index = 19;
                break;
            case 'U':
                index = 20;
                break;
            case 'V':
                index = 21;
                break;
            case 'W':
                index = 22;
                break;
            case 'X':
                index = 23;
                break;
            case 'Y':
                index = 24;
                break;
            case 'Z':
                index = 25;
                break;
        }
        if (index != -1)
        {
            if(GameManager.Instance.tile_scores_int == 0)
            {
                tile_letter = tiles[index];
            }
            else if(GameManager.Instance.tile_scores_int == 1)
            {
                tile_letter = tiles_1s[index];
            }
			imageComponent.sprite = tile_letter;
		}
	}
}
