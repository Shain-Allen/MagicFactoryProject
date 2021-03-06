using UnityEngine;
using static ICHelpers;

/* See Base Class for further documentation for all override functions */
public class FullChestLogic : ItemControl, IOpenMenu
{
	[SerializeField]
	private ItemSpawnerMenu FullChestMenu;
	public GameObject itemToClone;

	public override void PlacedAction(GridControl grid_)
	{
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
