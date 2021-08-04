using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static HelpFuncs;
using static ItemControlHelpers;

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

		if (Input.GetMouseButtonDown(0))
			DetectPlacing();
		if (Input.GetMouseButtonDown(1))
			DetectDestroying();

		// Move the camera and player
		cam.transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
		UpdatePlayerAnimation();

		//See if new chunks need to be loaded or unloaded
		Vector2Int bottomLeftBound = ChunkManager.getBottomLeftBound(cam);
		Vector2Int topRightBound = ChunkManager.getTopRightBound(cam);
		ChunkManager.LoadChunks(grid, bottomLeftBound, topRightBound);
		ChunkManager.UnloadChunks(grid, bottomLeftBound, topRightBound);
	}

	// If the user is left clicks, place their held Placeable
	private void DetectPlacing()
	{
		if (brushItem != null && !EventSystem.current.IsPointerOverGameObject())
		{
			if (GetPlaceableAt(grid, roundedMousePos) != null)
				return;
			GameObject temp = Instantiate(brushItem, worldGrid.CellToWorld(worldGrid.WorldToCell(roundedMousePos)), itemPreview.transform.rotation, worldGrid.transform);
			temp.GetComponent<Placeable>().PlacedAction(grid);
		}
	}

	// If the user right clicks, destory the targeted Placeable
	private void DetectDestroying()
	{
		Placeable placeableToRemove = GetPlaceableAt(grid, roundedMousePos);
		if (placeableToRemove == null)
			return;
		placeableToRemove.RemovedAction();
	}

	// Changes the state of the Player animator so it animates correctly
	private void UpdatePlayerAnimation()
	{
		bool isWalking = !(moveInput.x == 0 && moveInput.y == 0);
		animator.SetBool("isWalking", isWalking);

		if (isWalking)
		{
			float facingFloat = VectorToEuler(moveInput) / 90;
			if (facingFloat % 2 != 0)
				facingFloat = (facingFloat < 2) ? 1 : 3;
			animator.SetInteger("facingDirection", (int)facingFloat);
		}
	}

	// Updates the brush item to the last clicked item
	public void ChangeBrushItem(int _itemID)
	{
		brushItem = ObjectDictionary.itemList[_itemID];
		itemPreview.sprite = brushItem.GetComponent<SpriteRenderer>().sprite;
		itemPreview.color = brushItem.GetComponent<SpriteRenderer>().color;
	}

	// Empties the current item from the brush
	public void EmptyBrushItem()
	{
		brushItem = null;
		itemPreview.sprite = null;
	}

	// Creates a movespeed based on WASD being pressed
	private void PlayerMovementStart(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	// Sets movement to 0,0 when WASD is not being pressed
	private void PlayerMovementStop()
	{
		moveInput = Vector2.zero;
	}

	// Rotates the brush item 90 degrees clockwise
	private void RotateItem()
	{
		if (itemPreview)
			itemPreview.transform.Rotate(new Vector3(0, 0, 1), 270f);
	}
}
