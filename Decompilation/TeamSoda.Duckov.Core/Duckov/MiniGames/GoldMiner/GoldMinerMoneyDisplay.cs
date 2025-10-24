using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029D RID: 669
	public class GoldMinerMoneyDisplay : MonoBehaviour
	{
		// Token: 0x060015DA RID: 5594 RVA: 0x00050DF0 File Offset: 0x0004EFF0
		private void Update()
		{
			this.text.text = this.master.Money.ToString(this.format);
		}

		// Token: 0x04001023 RID: 4131
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001024 RID: 4132
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001025 RID: 4133
		[SerializeField]
		private string format = "$0";
	}
}
