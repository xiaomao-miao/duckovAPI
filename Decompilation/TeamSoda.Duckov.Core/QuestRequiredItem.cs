using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000122 RID: 290
public class QuestRequiredItem : MonoBehaviour
{
	// Token: 0x06000989 RID: 2441 RVA: 0x00029728 File Offset: 0x00027928
	public void Set(int itemTypeID, int count = 1)
	{
		if (itemTypeID <= 0 || count <= 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
		if (metaData.id == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.icon.sprite = metaData.icon;
		this.text.text = string.Format("{0} x{1}", metaData.DisplayName, count);
		base.gameObject.SetActive(true);
	}

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private Image icon;

	// Token: 0x04000867 RID: 2151
	[SerializeField]
	private TextMeshProUGUI text;
}
