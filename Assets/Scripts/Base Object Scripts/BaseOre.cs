using UnityEngine;
using static GridHelpers;
using static ResourceHelpers;

public class BaseOre : BaseResource
{
	public OreInfoSO oreInfo;
	public int remainingOre;

	private GridControl grid;

	public void GenerateOre(GridControl _grid, GameObject chunkParent)
	{
		remainingOre = Mathf.RoundToInt(oreInfo.baseOreAmount + oreInfo.DistanceMultiplier * Vector3.Distance(transform.position, Vector3.zero));
		grid = _grid;

		chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(transform.position).x, PosToPosInChunk(transform.position).y] = gameObject;
	}

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