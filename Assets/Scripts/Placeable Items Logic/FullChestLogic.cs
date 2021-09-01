using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class FullChestLogic : ItemControl, IOpenMenu
{
	[SerializeField]
	private ItemSpawnerMenu FullChestMenu;
	public GameObject itemToClone;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowBackBelt = false;

		AddToWorld(grid, this);
		TryAttachFrontBelt();

		FullChestMenu = GameObject.FindGameObjectWithTag("ItemSpawnerMenu").GetComponent<ItemSpawnerMenu>();
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		if (pullingIC && pullingIC.AllowItem(this) && itemToClone)
		{
			GameObject createdItem = Instantiate(itemToClone, transform.position, Quaternion.identity, grid.transform);
			pullingIC.setItemSlot(this, createdItem);
			StartCoroutine(SmoothMove(grid, createdItem, transform.position, pullingIC.transform.position));
		}
	}

	public void OpenMenu()
	{
		FullChestMenu.transform.GetChild(0).gameObject.SetActive(true);
		FullChestMenu.SetCurrentItem(itemToClone.GetComponent<SpriteRenderer>().sprite, itemToClone.GetComponent<SpriteRenderer>().color);
		FullChestMenu.ConnectMenu(gameObject);
	}

	public void SetSpawnItem(GameObject item)
	{
		itemToClone = item;
		FullChestMenu.SetCurrentItem(itemToClone.GetComponent<SpriteRenderer>().sprite, itemToClone.GetComponent<SpriteRenderer>().color);
	}
}
