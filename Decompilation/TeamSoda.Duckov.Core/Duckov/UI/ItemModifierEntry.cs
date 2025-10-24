using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000393 RID: 915
	public class ItemModifierEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002035 RID: 8245 RVA: 0x000709A7 File Offset: 0x0006EBA7
		public void NotifyPooled()
		{
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000709A9 File Offset: 0x0006EBA9
		public void NotifyReleased()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000709B1 File Offset: 0x0006EBB1
		internal void Setup(ModifierDescription target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000709CC File Offset: 0x0006EBCC
		private void Refresh()
		{
			this.displayName.text = this.target.DisplayName;
			StatInfoDatabase.Entry entry = StatInfoDatabase.Get(this.target.Key);
			this.value.text = this.target.GetDisplayValueString(entry.DisplayFormat);
			Color color = this.color_Neutral;
			Polarity polarity = entry.polarity;
			if (this.target.Value != 0f)
			{
				switch (polarity)
				{
				case Polarity.Negative:
					color = ((this.target.Value < 0f) ? this.color_Positive : this.color_Negative);
					break;
				case Polarity.Positive:
					color = ((this.target.Value > 0f) ? this.color_Positive : this.color_Negative);
					break;
				}
			}
			this.value.color = color;
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00070AA3 File Offset: 0x0006ECA3
		private void RegisterEvents()
		{
			ModifierDescription modifierDescription = this.target;
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00070AAC File Offset: 0x0006ECAC
		private void UnregisterEvents()
		{
			ModifierDescription modifierDescription = this.target;
		}

		// Token: 0x040015F8 RID: 5624
		private ModifierDescription target;

		// Token: 0x040015F9 RID: 5625
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040015FA RID: 5626
		[SerializeField]
		private TextMeshProUGUI value;

		// Token: 0x040015FB RID: 5627
		[SerializeField]
		private Color color_Neutral;

		// Token: 0x040015FC RID: 5628
		[SerializeField]
		private Color color_Positive;

		// Token: 0x040015FD RID: 5629
		[SerializeField]
		private Color color_Negative;
	}
}
