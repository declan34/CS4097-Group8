using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScript : MonoBehaviour
{
    public static DrawScript Instance;
	[SerializeField] private Canvas canvas;
    public GameObject tilePrefab;

    public List<char> bag;

    // Start is called before the first frame update
    void Start()
    {
        play_tiles();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReDraw()
    {
        for(int i = 0; i < 7; i++)
        {
            //after removing the previous element, all elements have index -= 1
            //making the next element to be removed at index 0
            bag.Add(GameManager.Instance.tiles_on_rack[0].name[0]);
            //adds tiles back into tiles left counter
            ScoreSystem.Instance.addTiles(1);
            Destroy(GameManager.Instance.tiles_on_rack[0]);
            GameManager.Instance.tiles_on_rack.RemoveAt(0);
        }
        Shuffle(bag);
        DealTiles();
        return;
    }

    public void computerDraw()
    {
        for (int i = 0; i < 7; i++)
        {
            //after removing the previous element, all elements have index -= 1
            //making the next element to be removed at index 0
            bag.Add(GameManager.Instance.computer_hand[0].name[0]);
            //adds tiles back into tiles left counter
            ScoreSystem.Instance.addTiles(1);
            Destroy(GameManager.Instance.computer_hand[0]);
            GameManager.Instance.computer_hand.RemoveAt(0);
        }
        Shuffle(bag);
        DealComputer(7);
        return;
    }

    public void FillRack(int index)
    {
        float xOffset = -4.5f;
        float yOffset = -9.5f;
        float zOffset = 0.01f;

        xOffset = xOffset + (index * 1.5f);

        GameObject newtile = Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + zOffset), Quaternion.identity);
        newtile.name = bag[0].ToString();
        Debug.Log(bag[0].ToString());
        bag.RemoveAt(0);
        newtile.transform.SetParent(canvas.transform, true);
        newtile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ScoreSystem.Instance.subtractTiles(1);
        GameManager.Instance.tiles_on_rack.Add(newtile);

        return;
    }

    public void play_tiles()
    {
        bag = generate_bag();
        Shuffle(bag);

        //testing
        //foreach(char tile in bag)
        //{
          //  print(tile);
        //}
        DealTiles();
        DealComputer(7);
        return;
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

    public void Shuffle<T>(List<T> list)
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
        return;
    }
        
    public void DealTiles()
    {
        float xOffset = -4.5f;
        float yOffset = -9.5f;
        float zOffset = 0.01f;
        for(int i = 0; i < 7; i++)
        {
            if (bag.Count == 0) break;
            GameObject newtile = Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + zOffset), Quaternion.identity);
            newtile.name = bag[0].ToString();
            Debug.Log(bag[0].ToString());
            bag.RemoveAt(0);
            newtile.transform.SetParent(canvas.transform, true);
            newtile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            ScoreSystem.Instance.subtractTiles(1);
            GameManager.Instance.tiles_on_rack.Add(newtile);

           xOffset = xOffset + 1.5f;
        }
        return;
    }

    public void DealComputer(int numTiles)
    {
        for(int i = 0; i < numTiles; i++)
        {
            if (bag.Count == 0) break;
            GameObject newtile = Instantiate(tilePrefab, new Vector3(transform.position.x + 100, transform.position.y + 100, transform.position.z), Quaternion.identity);
            newtile.name = bag[0].ToString();
            newtile.transform.SetParent(canvas.transform, true);
            newtile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            bag.RemoveAt(0);
            ScoreSystem.Instance.subtractTiles(1);
            GameManager.Instance.computer_hand.Add(newtile);
        }
        return;
    }
}
