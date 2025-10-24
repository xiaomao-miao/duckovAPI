using System;
using ItemStatsSystem;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000D2 RID: 210
public class WeightBarHUD : MonoBehaviour
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001D27F File Offset: 0x0001B47F
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0001D28C File Offset: 0x0001B48C
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
		float totalWeight = this.characterMainControl.CharacterItem.TotalWeight;
		float a = this.characterMainControl.MaxWeight;
		if (!Mathf.Approximately(totalWeight, this.weight) || !Mathf.Approximately(a, this.maxWeight))
		{
			this.weight = totalWeight;
			this.maxWeight = a;
			this.percent = this.weight / this.maxWeight;
			this.weightText.text = string.Format(this.weightTextFormat, this.weight, this.maxWeight);
			this.fillImage.fillAmount = this.percent;
			this.SetColor();
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0001D364 File Offset: 0x0001B564
	private void SetColor()
	{
		Color color;
		if (this.percent < 0.25f)
		{
			color = this.lightColor;
		}
		else if (this.percent < 0.75f)
		{
			color = this.normalColor;
		}
		else if (this.percent < 1f)
		{
			color = this.heavyColor;
		}
		else
		{
			color = this.overWeightColor;
		}
		float h;
		float num;
		float v;
		Color.RGBToHSV(color, out h, out num, out v);
		Color color2 = color;
		if (num > 0.4f)
		{
			num = 0.4f;
			v = 1f;
			color2 = Color.HSVToRGB(h, num, v);
		}
		this.glow.Color = color;
		this.fillImage.color = color2;
		this.weightText.color = color;
	}

	// Token: 0x0400063C RID: 1596
	private CharacterMainControl characterMainControl;

	// Token: 0x0400063D RID: 1597
	private float percent;

	// Token: 0x0400063E RID: 1598
	private float weight;

	// Token: 0x0400063F RID: 1599
	private float maxWeight;

	// Token: 0x04000640 RID: 1600
	public ProceduralImage fillImage;

	// Token: 0x04000641 RID: 1601
	public TrueShadow glow;

	// Token: 0x04000642 RID: 1602
	public Color lightColor;

	// Token: 0x04000643 RID: 1603
	public Color normalColor;

	// Token: 0x04000644 RID: 1604
	public Color heavyColor;

	// Token: 0x04000645 RID: 1605
	public Color overWeightColor;

	// Token: 0x04000646 RID: 1606
	public TextMeshProUGUI weightText;

	// Token: 0x04000647 RID: 1607
	public string weightTextFormat = "{0:0.#}/{1:0.#}kg";
}
