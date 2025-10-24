using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029C RID: 668
	public class EagleEyePotionDisplay : MonoBehaviour
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x00050D28 File Offset: 0x0004EF28
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00050D84 File Offset: 0x0004EF84
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
			this.amountText.text = string.Format("{0}", this.master.run.eagleEyePotion);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00050DD8 File Offset: 0x0004EFD8
		private void OnInteract(NavEntry entry)
		{
			this.master.UseEagleEyePotion();
		}

		// Token: 0x04001020 RID: 4128
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001021 RID: 4129
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x04001022 RID: 4130
		[SerializeField]
		private NavEntry navEntry;
	}
}
