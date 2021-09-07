using UnityEngine;

public class BaseGas : BaseResource
{
	public GasInfoSO gasInfo;
	public int remainingGas;

	public override void Generate(GridControl _grid)
	{
		remainingGas = Mathf.RoundToInt(gasInfo.baseGasAmount * gasInfo.DistanceMultiplier);
	}
}
