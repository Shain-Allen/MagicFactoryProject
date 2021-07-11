using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileBrush : MonoBehaviour
{
	//mouse pos stuff
	Vector3 mousePos;
	public Vector3 roundedMousePos;
	//the grid
	public Grid worldGrid;
	//the test tile
	public GameObject brushItem;
	//the preview Item for the brush
	public SpriteRenderer itemPreview;
	//the thing that holds all the items in the game
	public ItemDictionary itemDictionary;
	//holds a list of where all items are
	//List<Vector2> placedObjects = new List<Vector2>();
	Dictionary<Vector2, GameObject> placeObjects = new Dictionary<Vector2, GameObject>();

	// Update is called once per frame
	void Update()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mousePos.x, mousePos.y, 0);
		roundedMousePos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.Log(worldGrid.WorldToCell(roundedMousePos));
			//Debug.Log(worldGrid.CellToWorld(worldGrid.WorldToCell(roundedMousePos)));
			if (brushItem != null && (placeObjects.Count == 0 || !placeObjects.ContainsKey(roundedMousePos)))
			{
				placeObjects.Add(roundedMousePos, Instantiate(brushItem, worldGrid.CellToWorld(worldGrid.WorldToCell(roundedMousePos)), itemPreview.transform.rotation, worldGrid.transform));
				//Debug.Log("Placed");
				//Debug.Log(placeObjects.ContainsKey(roundedMousePos));
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			if (placeObjects.ContainsKey(roundedMousePos))
			{
				Debug.Log(placeObjects[roundedMousePos].transform.position);
				GameObject.Destroy(placeObjects[roundedMousePos]);
				placeObjects.Remove(roundedMousePos);
				Debug.Log("object Deleted");
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Rotate();
		}
	}

	public void ChangeBrushItem(int _itemID)
	{
		brushItem = itemDictionary.itemList[_itemID];
		itemPreview.sprite = brushItem.GetComponent<SpriteRenderer>().sprite;
		itemPreview.color = brushItem.GetComponent<SpriteRenderer>().color;
	}

	public void EmptyBrushItem()
	{
		brushItem = null;
		itemPreview.sprite = null;
	}

	public void Rotate()
	{
		itemPreview.transform.Rotate(new Vector3(0, 0, 1), 90f);
	}
}
