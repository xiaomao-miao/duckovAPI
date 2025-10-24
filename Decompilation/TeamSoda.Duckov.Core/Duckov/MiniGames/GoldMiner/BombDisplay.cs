using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029B RID: 667
	public class BombDisplay : MonoBehaviour
	{
		// Token: 0x060015D2 RID: 5586 RVA: 0x00050C60 File Offset: 0x0004EE60
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00050CBC File Offset: 0x0004EEBC
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
			this.amountText.text = string.Format("{0}", this.master.run.bomb);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00050D10 File Offset: 0x0004EF10
		private void OnInteract(NavEntry entry)
		{
			this.master.UseBomb();
		}

		// Token: 0x0400101D RID: 4125
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400101E RID: 4126
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x0400101F RID: 4127
		[SerializeField]
		private NavEntry navEntry;
	}
}
