using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScript : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
    public GameObject tilePrefab;


    public List<char> bag;
    public List<GameObject> tiles_in_play;
    public List<GameObject> tiles_on_rack;

    // Start is called before the first frame update
    void Start()
    {
        play_tiles();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            foreach(GameObject g in tiles_on_rack)
            {
                bag.Add(g.name[0]);
                tiles_on_rack.Remove(g);
                Destroy(g);
            }
            play_tiles();
        }
        enabled = false;
    }

    public void play_tiles()
    {
        bag = generate_bag();
        Shuffle(bag);

        //testing
       // foreach(char tile in bag)
        //{
          //  print(tile);
        //}
        DealTiles();
    }

    public static List<char> generate_bag()
    {
        List<char> new_bag = new List<char>();
        foreach(char t in GameManager.tile_letters)
        {
            new_bag.Add(t);
        }
        return new_bag;
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n  = list.Count;
        while(n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    void DealTiles()
    {
        float xOffset = -4.5f;
        float yOffset = -9.5f;
        float zOffset = 0.01f;
        for(int i = 0; i < 7; i++)
        {
            GameObject newtile = Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + zOffset), Quaternion.identity);
            newtile.name = bag[i].ToString();
            bag.RemoveAt(i);
            newtile.transform.SetParent(canvas.transform, true);
            newtile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            ScoreSystem.Instance.subtractTiles(1);
            tiles_on_rack.Add(newtile);

            xOffset = xOffset + 1.5f;
        }

    }
}
