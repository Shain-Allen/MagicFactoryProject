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

	[SerializeField]
	private GameObject CurrentItem;

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

	public void SetCurrentItem(Sprite item, Color color)
	{
		CurrentItem.GetComponent<Image>().sprite = item;
		CurrentItem.GetComponent<Image>().color = color;
	}

	public void disconnectMenu()
	{
		if (selectionPanel.transform.childCount == 0)
			return;

		for (int i = 0; i < selectionPanel.transform.childCount; i++)
		{
			selectionPanel.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
		}
	}

	public void ConnectMenu(GameObject fullchest)
	{
		if (selectionPanel.transform.childCount == 0)
			return;

		for (int i = 0; i < selectionPanel.transform.childCount; i++)
		{
			GameObject item = itemDictionary.itemList[i];
			selectionPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => fullchest.GetComponent<FullChestLogic>().SetSpawnItem(item));
		}
	}
}
