using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;
using static HelpFuncs;

public class BaseOre : MonoBehaviour
{
	public OreInfoSO oreInfo;
	public int remainingOre;

	private GridControl grid;

	public void GenerateOre(GridControl _grid)
	{
		remainingOre = Mathf.RoundToInt(oreInfo.baseOreAmount * oreInfo.DistanceMultiplier);
		grid = _grid;
	}

	public void MineOre(out GameObject returnOre)
	{
		remainingOre--;
		returnOre = oreInfo.itemDictionary.itemList[oreInfo.returnObjectIndex];
		GameObject chunkParent;
		if (remainingOre == 0 && grid.worldChunks.TryGetValue(GetChunk(transform.position), out chunkParent))
		{
			chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(transform.position).x, PosToPosInChunk(transform.position).y] = null;
			Destroy(gameObject);
		}
	}
}