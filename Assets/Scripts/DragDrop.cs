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

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
	}
	public void OnBeginDrag(PointerEventData eventData)
    {
		Debug.Log("OnBeginDrag");
		canvasGroup.alpha = 0.6f;
		canvasGroup.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
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
		Debug.Log(this.GetComponent<Tile>().tileObject.location);
		if (this.GetComponent<Tile>().tileObject.location != (-1, -1))
		{
			(int x, int y) = this.GetComponent<Tile>().tileObject.location;
			GameManager.Instance.Board[x, y].letter = ' ';
			this.GetComponent<Tile>().changeLocation((-1, -1));
		}
	}
}
