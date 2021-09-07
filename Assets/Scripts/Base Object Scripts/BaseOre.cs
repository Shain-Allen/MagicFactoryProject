using UnityEngine;
using static ResourceHelpers;

public class BaseOre : BaseResource
{
	public OreInfoSO oreInfo;
	public int remainingOre;
	private GridControl grid;

	// Creates this ore and initalized how many ore remains in it
	public override void Generate(GridControl grid_)
	{
		grid = grid_;
		remainingOre = Mathf.RoundToInt(oreInfo.baseOreAmount + oreInfo.DistanceMultiplier * Vector3.Distance(transform.position, Vector3.zero));
		AddToWorld(grid, this);
	}

	// Substracts 1 from the remaining ore, returnOre returns the type of ore this makes, then this deletes if it runs empty
	public void MineOre(out GameObject returnOre)
	{
		remainingOre--;
		returnOre = oreInfo.itemDictionary.itemList[oreInfo.returnObjectIndex];
		if (remainingOre == 0)
		{
			RemoveResourceFromWorld(grid, this);
			Destroy(gameObject);
		}
	}
}