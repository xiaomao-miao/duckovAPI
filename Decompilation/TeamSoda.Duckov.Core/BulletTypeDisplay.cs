using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class BulletTypeDisplay : MonoBehaviour
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0003A75A File Offset: 0x0003895A
	[LocalizationKey("Default")]
	private string NotAssignedTextKey
	{
		get
		{
			return "UI_Bullet_NotAssigned";
		}
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0003A764 File Offset: 0x00038964
	internal void Setup(int targetBulletID)
	{
		if (targetBulletID < 0)
		{
			this.bulletDisplayName.text = this.NotAssignedTextKey.ToPlainText();
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(targetBulletID);
		this.bulletDisplayName.text = metaData.DisplayName;
	}

	// Token: 0x04000C15 RID: 3093
	[SerializeField]
	private TextMeshProUGUI bulletDisplayName;
}
