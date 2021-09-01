using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static GridHelpers;
using static PlaceableHelpers;

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
		gameControls.GeneralControls.LeftClick.started += ctx => LeftClickStart();
		gameControls.GeneralControls.RightClick.started += ctx => RightClickStart();
	}

	// Update is called once per frame
	void Update()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mousePos.x, mousePos.y, 0);
		roundedMousePos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

		// Move the camera and player
		cam.transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;

		//See if new chunks need to be loaded or unloaded
		Vector2Int bottomLeftBound = ChunkManager.getBottomLeftBound(cam);
		Vector2Int topRightBound = ChunkManager.getTopRightBound(cam);
		ChunkManager.LoadChunks(grid, bottomLeftBound, topRightBound);
		// ChunkManager.UnloadChunks(grid, bottomLeftBound, topRightBound);
	}

	// If the user is left clicks, place their held Placeable
	private bool TryToPlace()
	{
		if (brushItem && !EventSystem.current.IsPointerOverGameObject())
		{
			if (GetPlaceableAt<Placeable>(grid, roundedMousePos) != null)
				return false;
			GameObject tempChunkParent = GetChunkParentByPos(grid, roundedMousePos);
			GameObject tempPlaceable = Instantiate(brushItem, roundedMousePos, itemPreview.transform.rotation, tempChunkParent.transform);
			tempPlaceable.GetComponent<Placeable>().PlacedAction(grid);
			return true;
		}
		return false;
	}

	// If the user right clicks, destory the targeted Placeable
	private void TryToDestroy()
	{
		Placeable placeableToRemove = GetPlaceableAt<Placeable>(grid, roundedMousePos);
		if (placeableToRemove == null)
			return;
		placeableToRemove.RemovedAction();
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

		animator.SetBool("isWalking", true);

		animator.SetFloat("MoveX", moveInput.x);
		animator.SetFloat("MoveY", moveInput.y);
	}

	// Sets movement to 0,0 when WASD is not being pressed
	private void PlayerMovementStop()
	{
		moveInput = Vector2.zero;

		animator.SetBool("isWalking", false);
	}

	// Rotates the brush item 90 degrees clockwise
	private void RotateItem()
	{
		if (itemPreview)
			itemPreview.transform.Rotate(new Vector3(0, 0, 1), 270f);
	}

	private void LeftClickStart()
	{
		if (!TryToPlace())
		{
			IOpenMenu placeable;
			if ((placeable = GetPlaceableAt<Placeable>(grid, roundedMousePos) as IOpenMenu) != null && !brushItem)
			{
				placeable.OpenMenu();
			}
		}
	}

	private void RightClickStart()
	{
		TryToDestroy();
	}
}
