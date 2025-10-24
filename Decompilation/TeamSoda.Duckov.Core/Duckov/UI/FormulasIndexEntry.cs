using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000383 RID: 899
	public class FormulasIndexEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x0006D1AC File Offset: 0x0006B3AC
		public CraftingFormula Formula
		{
			get
			{
				return this.formula;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x0006D1B4 File Offset: 0x0006B3B4
		private int ItemID
		{
			get
			{
				return this.formula.result.id;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0006D1C6 File Offset: 0x0006B3C6
		private ItemMetaData Meta
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.ItemID);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001F25 RID: 7973 RVA: 0x0006D1D3 File Offset: 0x0006B3D3
		private bool Unlocked
		{
			get
			{
				return CraftingManager.IsFormulaUnlocked(this.formula.id);
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0006D1E5 File Offset: 0x0006B3E5
		public bool Valid
		{
			get
			{
				return this.ItemID >= 0;
			}
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0006D1F3 File Offset: 0x0006B3F3
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.OnEntryClicked(this);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0006D201 File Offset: 0x0006B401
		internal void Setup(FormulasIndexView master, CraftingFormula formula)
		{
			this.master = master;
			this.formula = formula;
			this.Refresh();
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0006D218 File Offset: 0x0006B418
		public void Refresh()
		{
			ItemMetaData meta = this.Meta;
			if (!this.Valid)
			{
				this.displayNameText.text = "! " + this.formula.id + " !";
				this.image.sprite = this.lockedImage;
				return;
			}
			if (this.Unlocked)
			{
				this.displayNameText.text = string.Format("{0} x{1}", meta.DisplayName, this.formula.result.amount);
				this.image.sprite = meta.icon;
				return;
			}
			this.displayNameText.text = this.lockedText;
			this.image.sprite = this.lockedImage;
		}

		// Token: 0x04001546 RID: 5446
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001547 RID: 5447
		[SerializeField]
		private Image image;

		// Token: 0x04001548 RID: 5448
		[SerializeField]
		private string lockedText = "???";

		// Token: 0x04001549 RID: 5449
		[SerializeField]
		private Sprite lockedImage;

		// Token: 0x0400154A RID: 5450
		private FormulasIndexView master;

		// Token: 0x0400154B RID: 5451
		private CraftingFormula formula;
	}
}
