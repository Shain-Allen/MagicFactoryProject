using UnityEngine;

/* See Base Class for further documentation for all override functions */
public class BaseGas : BaseResource
{
	public GasInfoSO gasInfo;

	public override void Generate(GridControl grid_)
	{
		base.Generate(grid_);
		remainingResource = Mathf.RoundToInt(gasInfo.baseGasAmount * gasInfo.DistanceMultiplier);
	}

	public override void Extract(out GameObject returnOutput)
	{
		remainingResource--;
		returnOutput = gasInfo.itemDictionary.itemList[gasInfo.returnObjectIndex];
		if (remainingResource == 0)
			Delete();
	}
}
