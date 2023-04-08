using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateSprite : MonoBehaviour
{
	public Sprite[] tiles;
	private Sprite tile_letter;
    private Image imageComponent;
    public bool OnRack;

    // Start is called before the first frame update
    void Start()
    {
		imageComponent = GetComponent<Image>();

		int index = System.Array.IndexOf(GameManager.tile_letters, char.Parse(name));
        //Debug.Log(name);
        //Debug.Log(index);
		tile_letter = tiles[index];
		imageComponent.sprite = tile_letter;
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
