using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOre : MonoBehaviour
{
	public OreInfoSO oreInfo;
	public int remainingOre;

	/*public BaseOre(GameObject ore, GameObject outputItem, int remainingOre)
	{
		this.ore = ore;
		this.outputItem = outputItem;
		this.remainingOre = remainingOre;
	}*/

	public void GenerateOre()
	{
		remainingOre = Mathf.RoundToInt(oreInfo.baseOreAmount * oreInfo.DistanceMultiplier);
	}
}