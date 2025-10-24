using System;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class BulletTypeSelectButton : MonoBehaviour
{
	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001B2A9 File Offset: 0x000194A9
	public int BulletTypeID
	{
		get
		{
			return this.bulletTypeID;
		}
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x0001B2B1 File Offset: 0x000194B1
	public void SetSelection(bool selected)
	{
		this.selectShadow.enabled = selected;
		this.indicator.SetActive(selected);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001B2CB File Offset: 0x000194CB
	public void Init(int id, int count)
	{
		this.bulletTypeID = id;
		this.bulletCount = count;
		this.SetSelection(false);
		this.RefreshContent();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0001B2E8 File Offset: 0x000194E8
	public void RefreshContent()
	{
		this.nameText.text = this.GetBulletName(this.bulletTypeID);
		this.countText.text = this.bulletCount.ToString();
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0001B318 File Offset: 0x00019518
	public string GetBulletName(int id)
	{
		if (id > 0)
		{
			return ItemAssetsCollection.GetMetaData(id).DisplayName;
		}
		return "UI_Bullet_NotAssigned".ToPlainText();
	}

	// Token: 0x0400059D RID: 1437
	private int bulletTypeID;

	// Token: 0x0400059E RID: 1438
	private int bulletCount;

	// Token: 0x0400059F RID: 1439
	public BulletTypeHUD bulletTypeHUD;

	// Token: 0x040005A0 RID: 1440
	public TextMeshProUGUI nameText;

	// Token: 0x040005A1 RID: 1441
	public TextMeshProUGUI countText;

	// Token: 0x040005A2 RID: 1442
	public TrueShadow selectShadow;

	// Token: 0x040005A3 RID: 1443
	public GameObject indicator;
}
