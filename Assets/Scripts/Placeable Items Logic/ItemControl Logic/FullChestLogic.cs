using System;
using UnityEngine;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class FullChestLogic : ItemControl, IOpenMenu
{
	[SerializeField]
	private ItemSpawnerMenu FullChestMenu;
	public GameObject itemToClone;

	public override void PlacedAction(GridControl grid_)
	{
		allowInputs = false;
		outputValidRelPoses.Add(transform.up);
		base.PlacedAction(grid_);
		FullChestMenu = GameObject.FindGameObjectWithTag("ItemSpawnerMenu").GetComponent<ItemSpawnerMenu>();
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
