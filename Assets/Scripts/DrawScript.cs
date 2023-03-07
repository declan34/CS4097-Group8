using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScript : MonoBehaviour
{
    public Sprite[] tiles;
    public GameObject tilePrefab;
    public static string[] tile_letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", 
        "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    public static int[] points = new int[] { 1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10 };

    public List<string> bag;

    // Start is called before the first frame update
    void Start()
    {
        play_tiles();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play_tiles()
    {
        bag = generate_bag();
        Shuffle(bag);

        //testing
        foreach(string tile in bag)
        {
            print(tile);
        }
        DealTiles();
    }

    public static List<string> generate_bag()
    {
        List<string> new_bag = new List<string>();
        foreach(string t in tile_letters)
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
            newtile.name = bag[i];

            xOffset = xOffset + 1.5f;
        }

    }
}
