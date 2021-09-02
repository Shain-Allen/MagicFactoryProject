using UnityEngine;

public class BaseGas : BaseResource
{
	public GasInfoSO gasInfo;
	public int remainingGas;

	public void GenerateGas()
	{
		remainingGas = Mathf.RoundToInt(gasInfo.baseGasAmount * gasInfo.DistanceMultiplier);
	}
}
