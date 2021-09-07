using UnityEngine;
using static ResourceHelpers;

public class BaseGas : BaseResource
{
	public GasInfoSO gasInfo;
	public int remainingGas;
	private GridControl grid;

	public override void Generate(GridControl grid_)
	{
		grid = grid_;
		remainingGas = Mathf.RoundToInt(gasInfo.baseGasAmount * gasInfo.DistanceMultiplier);
		AddToWorld(grid, this);
	}
}
