using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BC RID: 188
public class HealthHUD : MonoBehaviour
{
	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001B411 File Offset: 0x00019611
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0001B420 File Offset: 0x00019620
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
		float num = this.characterMainControl.Health.MaxHealth;
		float currentHealth = this.characterMainControl.Health.CurrentHealth;
		float a = currentHealth / num;
		if (!Mathf.Approximately(a, this.percent))
		{
			this.percent = a;
			this.fillImage.fillAmount = this.percent;
			if (this.percent <= 0f)
			{
				this.backgroundImage.color = this.emptyBackgroundColor;
			}
			else
			{
				this.backgroundImage.color = this.backgroundColor;
			}
		}
		if (num != this.maxHealth || currentHealth != this.currenthealth)
		{
			this.maxHealth = num;
			this.currenthealth = currentHealth;
			this.text.text = this.currenthealth.ToString("0.#") + " / " + this.maxHealth.ToString("0.#");
		}
	}

	// Token: 0x040005AA RID: 1450
	private CharacterMainControl characterMainControl;

	// Token: 0x040005AB RID: 1451
	private float percent = -1f;

	// Token: 0x040005AC RID: 1452
	private float maxHealth;

	// Token: 0x040005AD RID: 1453
	private float currenthealth;

	// Token: 0x040005AE RID: 1454
	public ProceduralImage fillImage;

	// Token: 0x040005AF RID: 1455
	public ProceduralImage backgroundImage;

	// Token: 0x040005B0 RID: 1456
	public Color backgroundColor;

	// Token: 0x040005B1 RID: 1457
	public Color emptyBackgroundColor;

	// Token: 0x040005B2 RID: 1458
	public TextMeshProUGUI text;
}
