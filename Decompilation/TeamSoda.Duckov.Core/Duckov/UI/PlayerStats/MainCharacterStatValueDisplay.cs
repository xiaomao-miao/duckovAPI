using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI.PlayerStats
{
	// Token: 0x020003CB RID: 971
	public class MainCharacterStatValueDisplay : MonoBehaviour
	{
		// Token: 0x06002351 RID: 9041 RVA: 0x0007BA90 File Offset: 0x00079C90
		private void OnEnable()
		{
			if (this.target == null)
			{
				CharacterMainControl main = CharacterMainControl.Main;
				Stat stat;
				if (main == null)
				{
					stat = null;
				}
				else
				{
					Item characterItem = main.CharacterItem;
					stat = ((characterItem != null) ? characterItem.GetStat(this.statKey.GetHashCode()) : null);
				}
				this.target = stat;
			}
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0007BADF File Offset: 0x00079CDF
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0007BAE7 File Offset: 0x00079CE7
		private void AutoRename()
		{
			base.gameObject.name = "StatDisplay_" + this.statKey;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0007BB04 File Offset: 0x00079D04
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty += this.OnTargetDirty;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0007BB26 File Offset: 0x00079D26
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty -= this.OnTargetDirty;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0007BB48 File Offset: 0x00079D48
		private void OnTargetDirty(Stat stat)
		{
			this.Refresh();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0007BB50 File Offset: 0x00079D50
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.displayNameText.text = this.target.DisplayName;
			float value = this.target.Value;
			this.valueText.text = string.Format(this.format, value);
		}

		// Token: 0x04001804 RID: 6148
		[SerializeField]
		private string statKey;

		// Token: 0x04001805 RID: 6149
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001806 RID: 6150
		[SerializeField]
		private TextMeshProUGUI valueText;

		// Token: 0x04001807 RID: 6151
		[SerializeField]
		private string format = "{0:0.0}";

		// Token: 0x04001808 RID: 6152
		private Stat target;
	}
}
