using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BB RID: 187
public class EnergyHUD : MonoBehaviour
{
	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000611 RID: 1553 RVA: 0x0001B34A File Offset: 0x0001954A
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0001B358 File Offset: 0x00019558
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
		float a = this.characterMainControl.CurrentEnergy / this.characterMainControl.MaxEnergy;
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

	// Token: 0x040005A4 RID: 1444
	private CharacterMainControl characterMainControl;

	// Token: 0x040005A5 RID: 1445
	private float percent = -1f;

	// Token: 0x040005A6 RID: 1446
	public ProceduralImage fillImage;

	// Token: 0x040005A7 RID: 1447
	public ProceduralImage backgroundImage;

	// Token: 0x040005A8 RID: 1448
	public Color backgroundColor;

	// Token: 0x040005A9 RID: 1449
	public Color emptyBackgroundColor;
}
