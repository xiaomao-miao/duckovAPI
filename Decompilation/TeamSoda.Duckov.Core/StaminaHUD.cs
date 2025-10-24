using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BE RID: 190
public class StaminaHUD : MonoBehaviour
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001B5E7 File Offset: 0x000197E7
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0001B5F4 File Offset: 0x000197F4
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
		float a = this.characterMainControl.CurrentStamina / this.characterMainControl.MaxStamina;
		if (!Mathf.Approximately(a, this.percent))
		{
			this.percent = a;
			this.fillImage.fillAmount = this.percent;
			this.SetColor();
			if (Mathf.Approximately(a, 1f))
			{
				this.targetAlpha = 0f;
			}
			else
			{
				this.targetAlpha = 1f;
			}
		}
		this.UpdateAlpha(Time.unscaledDeltaTime);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0001B6A0 File Offset: 0x000198A0
	private void SetColor()
	{
		float h;
		float s;
		float v;
		Color.RGBToHSV(this.glowColor.Evaluate(this.percent), out h, out s, out v);
		s = 0.4f;
		v = 1f;
		Color color = Color.HSVToRGB(h, s, v);
		this.fillImage.color = color;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0001B6EA File Offset: 0x000198EA
	private void UpdateAlpha(float deltaTime)
	{
		if (this.targetAlpha != this.canvasGroup.alpha)
		{
			this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, this.targetAlpha, 5f * deltaTime);
		}
	}

	// Token: 0x040005B6 RID: 1462
	private CharacterMainControl characterMainControl;

	// Token: 0x040005B7 RID: 1463
	private float percent;

	// Token: 0x040005B8 RID: 1464
	public CanvasGroup canvasGroup;

	// Token: 0x040005B9 RID: 1465
	private float targetAlpha;

	// Token: 0x040005BA RID: 1466
	public ProceduralImage fillImage;

	// Token: 0x040005BB RID: 1467
	public Gradient glowColor;
}
