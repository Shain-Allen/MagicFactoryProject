using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "OreInfo", menuName = "OreInfoAsset")]
public class OreInfoSO : ScriptableObject
{
	[Min(1), Tooltip("the minimum amount of ore that can ever spawn even when right at 0,0 in the world")]
	public int baseOreAmount = 1;
	[Min(1), Tooltip("how quickly the amount of avalable ore will scale with distance from spawn")]
	public float DistanceMultiplier = 1;

	[Header("Autofilled fields (or at least they should be")]
	[Tooltip("this is where in the ItemDictionary to find the Item Version of this ore")]
	public int returnObjectIndex;
	[Tooltip("reference to the Dictionary asset that stores all the items in the game")]
	public ObjectDictionary itemDictionary;

	private void OnEnable()
	{
		itemDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/ItemDictionary");
	}

	private void OnValidate()
	{
		ObjectDictionary oreDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/OreDictionary");

		for (int i = 0; i < oreDictionary.itemList.Length; i++)
		{
			if (this.name.ToLower() == oreDictionary.itemList[i].name.ToLower())
			{
				returnObjectIndex = i;
			}
		}
	}
}
