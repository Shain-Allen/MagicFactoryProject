using UnityEngine;

/* See Base Class for further documentation for all override functions */
public class BaseOre : BaseResource
{
	public OreInfoSO oreInfo;

	public override void Generate(GridControl grid_)
	{
		base.Generate(grid_);
		remainingResource = Mathf.RoundToInt(oreInfo.baseOreAmount + oreInfo.DistanceMultiplier * Vector3.Distance(transform.position, Vector3.zero));
	}

	public override void Extract(out GameObject returnOutput)
	{
		remainingResource--;
		returnOutput = oreInfo.itemDictionary.itemList[oreInfo.returnObjectIndex];
		if (remainingResource == 0)
			Delete();
	}
}