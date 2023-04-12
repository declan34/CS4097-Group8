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
        //GameManager gmmgr = GameManager.Find("");
        //int tile_set = gmmgr.tile_scores_int;
        //GameManager.Instance.tile_scores_int;
		imageComponent = GetComponent<Image>();
		int index = System.Array.IndexOf(GameManager.tile_letters, name[0]);
        //Debug.Log(tile_set);
        //Debug.Log(index);
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
