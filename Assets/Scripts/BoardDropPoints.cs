using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using System;

public class BoardDropPoints : MonoBehaviour, IDropHandler
{
    private int rowIndex, columnIndex;
    private void Start()
    {
        string waypointNum = Regex.Match(this.name, @"\d+").Value;
        columnIndex = int.Parse(waypointNum) % 15;
        rowIndex = (int.Parse(waypointNum) - columnIndex) / 15;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && !eventData.pointerDrag.GetComponent<Tile>().tileObject.locked)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            // The tile dropped will always have a name that corresponds to it's letter
            char tile_name = eventData.pointerDrag.name[0];
            // Using the index of the waypoint, change the board matrix
            GameManager.Instance.Board[rowIndex, columnIndex].letter = tile_name;
            int rackIndex = GameManager.Instance.tiles_on_rack.FindIndex(x => x.name == tile_name.ToString());

            eventData.pointerDrag.GetComponent<Tile>().changeLocation((rowIndex, columnIndex));
            //Debug.Log(eventData.pointerDrag.GetComponent<Tile>().tileObject.location);

            //GameManager.Instance.tiles_on_rack[rackIndex].location = (rowIndex, columnIndex);
            //Debug.Log(GameManager.Instance.tiles_on_rack[rackIndex].location);

            //Debug.Log(tile_name);
            //Debug.Log(columnIndex); 
            //Debug.Log(rowIndex);
            //Debug.Log(GameManager.Instance.Board[rowIndex, columnIndex]);
        }
    }
}
