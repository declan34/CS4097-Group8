using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField] private Canvas canvas;
	private static int rowIndex, columnIndex;

	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;
	private Tile tileScript;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		tileScript = GetComponent<Tile>();
	}
	public void OnBeginDrag(PointerEventData eventData)
    {
		Debug.Log("OnBeginDrag");
		Debug.Log(tileScript.tileObject.locked);
		if (!tileScript.tileObject.locked)
		{
			canvasGroup.alpha = 0.6f;
			canvasGroup.blocksRaycasts = false;

			//Debug.Log(this.tileScript.tileObject.location);
			if (tileScript.tileObject.location != (-1, -1))
			{
				(int x, int y) = this.tileScript.tileObject.location;
				GameManager.Instance.Board[x, y].letter = ' ';
				this.tileScript.changeLocation((-1, -1));
			}
		}
		else
		{
			Debug.Log("This tile is locked in place");
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!tileScript.tileObject.locked)
		{
			rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		}
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag");
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
	}

	public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
		
	}
}
