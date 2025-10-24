using System;
using Duckov.Economy;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B4 RID: 436
public class FormulasDetailsDisplay : MonoBehaviour
{
	// Token: 0x06000CE7 RID: 3303 RVA: 0x00035B7B File Offset: 0x00033D7B
	private void SetupEmpty()
	{
		this.contentFadeGroup.Hide();
		this.placeHolderFadeGroup.Show();
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x00035B93 File Offset: 0x00033D93
	private void SetupFormula(CraftingFormula formula)
	{
		this.formula = formula;
		this.RefreshContent();
		this.contentFadeGroup.Show();
		this.placeHolderFadeGroup.Hide();
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x00035BB8 File Offset: 0x00033DB8
	private void RefreshContent()
	{
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.formula.result.id);
		this.nameText.text = metaData.DisplayName;
		this.descriptionText.text = metaData.Description;
		this.image.sprite = metaData.icon;
		this.costDisplay.Setup(this.formula.cost, 1);
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x00035C27 File Offset: 0x00033E27
	public void Setup(CraftingFormula? formula)
	{
		if (formula == null)
		{
			this.SetupEmpty();
			return;
		}
		if (!CraftingManager.IsFormulaUnlocked(formula.Value.id))
		{
			this.SetupUnknown();
			return;
		}
		this.SetupFormula(formula.Value);
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x00035C60 File Offset: 0x00033E60
	private void SetupUnknown()
	{
		this.nameText.text = "???";
		this.descriptionText.text = "???";
		this.image.sprite = this.unknownImage;
		this.contentFadeGroup.Show();
		this.placeHolderFadeGroup.Hide();
		this.costDisplay.Setup(default(Cost), 1);
	}

	// Token: 0x04000B1D RID: 2845
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000B1E RID: 2846
	[SerializeField]
	private Image image;

	// Token: 0x04000B1F RID: 2847
	[SerializeField]
	private TextMeshProUGUI descriptionText;

	// Token: 0x04000B20 RID: 2848
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000B21 RID: 2849
	[SerializeField]
	private FadeGroup contentFadeGroup;

	// Token: 0x04000B22 RID: 2850
	[SerializeField]
	private FadeGroup placeHolderFadeGroup;

	// Token: 0x04000B23 RID: 2851
	[SerializeField]
	private Sprite unknownImage;

	// Token: 0x04000B24 RID: 2852
	private CraftingFormula formula;
}
