using UnityEngine;

[CreateAssetMenu(fileName = "GasInfo", menuName = "GasInfoAsset")]
public class GasInfoSO : ScriptableObject
{
	[Min(1), Tooltip("the minimum amount of gas that can ever spawn even when right at 0,0 in the world")]
	public int baseGasAmount = 1;
	[Min(1), Tooltip("how quickly the amount of avalable gas will scale with distance from spawn")]
	public float DistanceMultiplier = 1;

	[Header("Autofilled fields (or at least they should be")]
	[Tooltip("this is where in the ItemDictionary to find the Item Version of this gas")]
	public int returnObjectIndex;
	[Tooltip("reference to the Dictionary asset that stores all the items in the game")]
	public ObjectDictionary itemDictionary;

	private void OnEnable()
	{
		itemDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/ItemDictionary");
	}

	private void OnValidate()
	{
		if (this.name == "GasInfo")
			return;

		ObjectDictionary gasDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/GasDictionary");

		for (int i = 0; i < gasDictionary.itemList.Length; i++)
		{
			if (this.name.ToLower() == gasDictionary.itemList[i].name.ToLower())
			{
				returnObjectIndex = i;
			}
		}
	}
}
