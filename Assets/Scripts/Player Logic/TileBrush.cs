using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static HelpFuncs;
using static ItemControlHelpers;

public class TileBrush : MonoBehaviour
{
	// ObjectDicionary holds all items in the game
	public ObjectDictionary ObjectDictionary;
	GameControls gameControls;
	Vector2 moveInput;
	public float moveSpeed;
	public GameObject Player;
	Animator animator;
	GameObject cam;
	Vector3 mousePos;
	public Vector3 roundedMousePos;
	public Grid worldGrid;
	public GameObject brushItem;
	public SpriteRenderer itemPreview;
	public GridControl grid;

	private void Start()
	{
		cam = Camera.main.gameObject;
		animator = Player.GetComponent<Animator>();

		// Enable and attach inputs
		gameControls = new GameControls();
		gameControls.Enable();
		gameControls.GeneralControls.PlayerMovement.performed += ctx => PlayerMovementStart(ctx);
		gameControls.GeneralControls.PlayerMovement.canceled += ctx => PlayerMovementStop();
		gameControls.GeneralControls.Rotate.started += ctx => RotateItem();
		gameControls.GeneralControls.ClearBrush.started += ctx => EmptyBrushItem();
	}

	// Update is called once per frame
	void Update()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mousePos.x, mousePos.y, 0);
		roundedMousePos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

		if (Input.GetMouseButtonDown(0))
			TryToPlace();
		if (Input.GetMouseButtonDown(1))
			TryToDestroy();

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
	private void TryToPlace()
	{
		if (brushItem != null && !EventSystem.current.IsPointerOverGameObject())
		{
			if (GetPlaceableAt(grid, roundedMousePos) != null)
				return;
			GameObject tempChunkParent = GetChunkParentByPos(grid, roundedMousePos);
			GameObject tempPlaceable = Instantiate(brushItem, roundedMousePos, itemPreview.transform.rotation, tempChunkParent.transform);
			tempPlaceable.GetComponent<Placeable>().PlacedAction(grid);
		}
	}

	// If the user right clicks, destory the targeted Placeable
	private void TryToDestroy()
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
