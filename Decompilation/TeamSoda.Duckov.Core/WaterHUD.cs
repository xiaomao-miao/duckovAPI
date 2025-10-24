using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BF RID: 191
public class WaterHUD : MonoBehaviour
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001B72F File Offset: 0x0001992F
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0001B73C File Offset: 0x0001993C
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
		}
		float a = this.characterMainControl.CurrentWater / this.characterMainControl.MaxWater;
		if (!Mathf.Approximately(a, this.percent))
		{
			this.percent = a;
			this.fillImage.fillAmount = this.percent;
			if (this.percent <= 0f)
			{
				this.backgroundImage.color = this.emptyBackgroundColor;
				return;
			}
			this.backgroundImage.color = this.backgroundColor;
		}
	}

	// Token: 0x040005BC RID: 1468
	private CharacterMainControl characterMainControl;

	// Token: 0x040005BD RID: 1469
	private float percent = -1f;

	// Token: 0x040005BE RID: 1470
	public ProceduralImage fillImage;

	// Token: 0x040005BF RID: 1471
	public ProceduralImage backgroundImage;

	// Token: 0x040005C0 RID: 1472
	public Color backgroundColor;

	// Token: 0x040005C1 RID: 1473
	public Color emptyBackgroundColor;
}
