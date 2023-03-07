using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateSprite : MonoBehaviour
{
    public Sprite tile_letter;
    private SpriteRenderer spriteRenderer;
    private DrawScript drawscript;

    // Start is called before the first frame update
    void Start()
    {
        List<string> bag = DrawScript.generate_bag();
        drawscript = FindObjectOfType<DrawScript>();

        int i = 0;
        foreach (string s in bag)
        {
            if( this.name == s)
            {
                tile_letter = drawscript.tiles[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = tile_letter;
    }
}
