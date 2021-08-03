using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGas : MonoBehaviour
{
	public GasInfoSO gasInfo;
	public int remainingGas;

	public void GenerateGas()
	{
		remainingGas = Mathf.RoundToInt(gasInfo.baseGasAmount * gasInfo.DistanceMultiplier);
	}
}
