using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AB RID: 683
	public class StrengthPotionDisplay : MonoBehaviour
	{
		// Token: 0x0600163C RID: 5692 RVA: 0x00052040 File Offset: 0x00050240
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0005209C File Offset: 0x0005029C
		private void OnEarlyLevelPlayTick(GoldMiner miner)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.run == null)
			{
				return;
			}
			this.amountText.text = string.Format("{0}", this.master.run.strengthPotion);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000520F0 File Offset: 0x000502F0
		private void OnInteract(NavEntry entry)
		{
			this.master.UseStrengthPotion();
		}

		// Token: 0x0400107A RID: 4218
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400107B RID: 4219
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x0400107C RID: 4220
		[SerializeField]
		private NavEntry navEntry;
	}
}
