using System;
using UnityEngine;
using static PlaceableHelpers;

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

	public override void MoveItem()
	{
		if (frontBelt && !frontBelt.getItemSlot() && itemToClone)
		{
			frontBelt.setItemSlot(Instantiate(itemToClone, transform.position, Quaternion.identity, grid.transform));
			StartCoroutine(SmoothMove(grid, frontBelt.getItemSlot(), transform.position, frontBelt.transform.position));
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
