using System;
using Duckov.Economy;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200037E RID: 894
	public class MoneyDisplay : MonoBehaviour
	{
		// Token: 0x06001F07 RID: 7943 RVA: 0x0006CDF2 File Offset: 0x0006AFF2
		private void Awake()
		{
			EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
			SavesSystem.OnSetFile += this.OnSaveFileChanged;
			this.Refresh();
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0006CE1C File Offset: 0x0006B01C
		private void OnDestroy()
		{
			EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
			SavesSystem.OnSetFile -= this.OnSaveFileChanged;
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0006CE40 File Offset: 0x0006B040
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0006CE48 File Offset: 0x0006B048
		private void Refresh()
		{
			this.text.text = EconomyManager.Money.ToString(this.format);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x0006CE73 File Offset: 0x0006B073
		private void OnMoneyChanged(long arg1, long arg2)
		{
			this.Refresh();
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0006CE7B File Offset: 0x0006B07B
		private void OnSaveFileChanged()
		{
			this.Refresh();
		}

		// Token: 0x0400153B RID: 5435
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400153C RID: 5436
		[SerializeField]
		private string format = "n0";
	}
}
