using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateSprite : MonoBehaviour
{
	public Sprite[] tiles;
    public Sprite[] tiles_1s;
    public Sprite[] tiles_max;
    public Sprite[] tiles_random;
	private Sprite tile_letter;
    private Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
		imageComponent = GetComponent<Image>();
		int index = System.Array.IndexOf(GameManager.tile_letters, name[0]);
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
            else if(GameManager.Instance.tile_scores_int == 2)
            {
                tile_letter = tiles_max[index];
            }
            else if(GameManager.Instance.tile_scores_int == 3)
            {
                tile_letter = tiles_random[index];
            }
			imageComponent.sprite = tile_letter;
		}
	}
}
