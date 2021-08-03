using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "OreInfo", menuName = "OreInfoAsset")]
public class OreInfoSO : ScriptableObject
{
	public int baseOreAmount = 1;
	public int DistanceMultiplyer = 1;
	public int returnObjectIndex;
	public ObjectDictionary itemDictionary;
	public ObjectDictionary oreDictionary;

	private void OnEnable()
	{
		itemDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/ItemDictionary");
		oreDictionary = (ObjectDictionary)Resources.Load("Scriptable_Object_Items/OreDictionary");

		for (int i = 0; i < oreDictionary.itemList.Length; i++)
		{
			if (this.name.ToLower() == oreDictionary.itemList[i].name.ToLower())
			{
				returnObjectIndex = i;
			}
		}
	}
}
