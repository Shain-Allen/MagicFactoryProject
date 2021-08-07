using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawnerMenu : MonoBehaviour
{
	[SerializeField]
	private ObjectDictionary itemDictionary;

	[SerializeField]
	private GameObject newItemButtonPrefab;

	[SerializeField]
	private GameObject selectionPanel;

	private void Start()
	{
		for (int i = 0; i < itemDictionary.itemList.Length; i++)
		{
			GameObject newButton = Instantiate(newItemButtonPrefab, selectionPanel.transform.position, Quaternion.identity, selectionPanel.transform);

			newButton.GetComponent<Image>().sprite = itemDictionary.itemList[i].GetComponent<SpriteRenderer>().sprite;
			newButton.GetComponent<Image>().color = itemDictionary.itemList[i].GetComponent<SpriteRenderer>().color;
			newButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = itemDictionary.itemList[i].name;
		}
	}
}
