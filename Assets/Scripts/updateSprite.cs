using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateSprite : MonoBehaviour
{
	public Sprite[] tiles;
	private Sprite tile_letter;
    private Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
		imageComponent = GetComponent<Image>();

		int index = System.Array.IndexOf(GameManager.tile_letters, name[0]);
        Debug.Log(name);
        Debug.Log(index);
		tile_letter = tiles[index];
		imageComponent.sprite = tile_letter;
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
