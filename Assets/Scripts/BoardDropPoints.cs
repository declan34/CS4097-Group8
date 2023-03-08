using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class BoardDropPoints : MonoBehaviour, IDropHandler
{
    private static int rowIndex, columnIndex;
	private void Start()
	{
        
	}

	public void OnDrop(PointerEventData eventData)
    {
		string waypointNum = Regex.Match(this.name, @"\d+").Value;
		columnIndex = int.Parse(waypointNum) % 15;
		rowIndex = (int.Parse(waypointNum) - columnIndex) / 15;

		Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            
            // The tile dropped will always have a name that corresponds to it's letter
            char tile_name = eventData.pointerDrag.name[0];
            // Using the index of the waypoint, change the board matrix
            GameManager.Instance.Board[rowIndex,columnIndex] = tile_name;
            Debug.Log(tile_name);
			Debug.Log(columnIndex); 
            Debug.Log(rowIndex);
            Debug.Log(GameManager.Instance.Board[rowIndex, columnIndex]);
		}
    }
}
