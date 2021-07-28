using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TileBrush : MonoBehaviour
{
	//movement input
	Vector2 moveInput;
	//movement speed
	public float moveSpeed = 1f;
	//camera Reference
	GameObject cam;
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
	public GridControl grid;

	private void Start()
	{
		//get the main camera in the scene
		cam = Camera.main.gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mousePos.x, mousePos.y, 0);
		roundedMousePos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

		//placement
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (brushItem != null && (grid.placeObjects.Count == 0 || !grid.placeObjects.ContainsKey(roundedMousePos)))
			{
				GameObject objectPlaceholder;
				//place the object in the world aligned to the grid and add it to the grids dictionary for easy lookup for other things
				grid.placeObjects.Add(roundedMousePos, objectPlaceholder = Instantiate(brushItem, worldGrid.CellToWorld(worldGrid.WorldToCell(roundedMousePos)), itemPreview.transform.rotation, worldGrid.transform));

				if (objectPlaceholder.GetComponent<Placeable>() != null)
				{
					objectPlaceholder.GetComponent<Placeable>().PlacedAction(grid);
				}
			}
		}

		// deconstruction
		if (Input.GetMouseButtonDown(1))
		{
			if (grid.placeObjects.ContainsKey(roundedMousePos))
			{
				//find trhe object in the grid dictionary and delete it and remove from said dictionary
				if (grid.placeObjects[roundedMousePos].GetComponent<Placeable>() != null)
				{
					grid.placeObjects[roundedMousePos].GetComponent<Placeable>().RemovedAction();
				}
				else
				{
					GameObject.Destroy(grid.placeObjects[roundedMousePos]);
					//Debug.Log($"{grid.placeObjects[roundedMousePos]} Destroyed");
					grid.placeObjects.Remove(roundedMousePos);
					//Debug.Log("object Deleted");
				}

			}
		}

		// rotate item preview
		if (Input.GetKeyDown(KeyCode.R))
		{
			itemPreview.transform.Rotate(new Vector3(0, 0, 1), 90f);
		}

		//move the camera;
		cam.transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed;
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

	public void PlayerMovement(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
}
