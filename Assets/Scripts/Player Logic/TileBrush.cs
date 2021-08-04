using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static HelpFuncs;

public class TileBrush : MonoBehaviour
{
	//GameControls Asset for input manager
	GameControls gameControls;
	//movement input
	Vector2 moveInput;
	//movement speed
	public float moveSpeed = 25f;
	public GameObject Player;
	private Animator animator;
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
	public ObjectDictionary ObjectDictionary;
	//holds a list of where all items are
	public GridControl grid;

	private void Start()
	{
		//get the main camera in the scene
		cam = Camera.main.gameObject;

		//set GameControls Variable to new instance
		gameControls = new GameControls();

		//this needs to happen for input to work
		gameControls.Enable();

		//Delegate with lambda expression to hook up the Method to the C# event for starting the movement of the camera
		gameControls.GeneralControls.PlayerMovement.performed += ctx => PlayerMovementStart(ctx);

		//Delegate with Lambda expression to hook up the Method to the C# event for reseting moveInput to 0,0 to stop camera movement when WASD is not longer being pressed
		gameControls.GeneralControls.PlayerMovement.canceled += ctx => PlayerMovementStop();

		//Delegate with Lambda expression to hook up the Method to the C# event for rotating items
		gameControls.GeneralControls.Rotate.started += ctx => RotateItem();

		//Delegate with Lambda Expression to hook up the Method to the C# event for clearing the brush
		gameControls.GeneralControls.ClearBrush.started += ctx => EmptyBrushItem();

		animator = Player.GetComponent<Animator>();
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
				//grid.placeObjects.Add(roundedMousePos, objectPlaceholder = Instantiate(brushItem, worldGrid.CellToWorld(worldGrid.WorldToCell(roundedMousePos)), itemPreview.transform.rotation, worldGrid.transform));

				objectPlaceholder = Instantiate(brushItem, worldGrid.CellToWorld(worldGrid.WorldToCell(roundedMousePos)), itemPreview.transform.rotation, worldGrid.transform);

				if (objectPlaceholder.GetComponent<Placeable>() != null)
				{
					objectPlaceholder.GetComponent<Placeable>().PlacedAction(grid);
				}
				else
				{
					//grid.placeObjects.Add(roundedMousePos, objectPlaceholder);
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

		//move the camera;
		cam.transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
		bool isWalking = true;
		int facingDirection = 3;
		if (moveInput.x == 0 && moveInput.y == 0)
			isWalking = false;
		else if (moveInput.x == 0)
		{
			if (moveInput.y > 0)
				facingDirection = 0;
			else
				facingDirection = 2;
		}
		else if (moveInput.x < 0)
			facingDirection = 1;
		else
			facingDirection = 3;
		animator.SetBool("isWalking", isWalking);
		animator.SetInteger("facingDirection", facingDirection);

		//See if new chunks need to be loaded or unloaded
		Vector2Int bottomLeftBound = ChunkManager.getBottomLeftBound(cam);
		Vector2Int topRightBound = ChunkManager.getTopRightBound(cam);
		ChunkManager.LoadChunks(grid, bottomLeftBound, topRightBound);
		ChunkManager.UnloadChunks(grid, bottomLeftBound, topRightBound);
	}

	//Update the brush item to the last clicked item
	public void ChangeBrushItem(int _itemID)
	{
		brushItem = ObjectDictionary.itemList[_itemID];
		itemPreview.sprite = brushItem.GetComponent<SpriteRenderer>().sprite;
		itemPreview.color = brushItem.GetComponent<SpriteRenderer>().color;
	}

	//Empty the current item from the Brush
	public void EmptyBrushItem()
	{
		brushItem = null;
		itemPreview.sprite = null;
	}

	//this is constantly updating MoveInput to reflect the state of WASD
	private void PlayerMovementStart(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	//this resets the MoveInput to 0,0 once WASD is not being pressed anymore to stop camera movement
	private void PlayerMovementStop()
	{
		moveInput = new Vector2(0, 0);
	}

	//this rotates the item when the rotate key is pressed
	private void RotateItem()
	{
		if (itemPreview)
		{
			itemPreview.transform.Rotate(new Vector3(0, 0, 1), 270f);
		}
	}
}
