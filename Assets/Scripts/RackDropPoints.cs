using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine;

public class RackDropPoints : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		Debug.Log("OnDrop");
		if (eventData.pointerDrag != null && !eventData.pointerDrag.GetComponent<Tile>().tileObject.locked)
		{
			eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

			eventData.pointerDrag.GetComponent<Tile>().changeLocation((-1, -1));
			Debug.Log(eventData.pointerDrag.GetComponent<Tile>().tileObject.location);
		}
	}
}
