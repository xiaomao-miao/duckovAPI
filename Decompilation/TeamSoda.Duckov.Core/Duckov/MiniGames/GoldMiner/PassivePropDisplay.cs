using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A7 RID: 679
	public class PassivePropDisplay : MonoBehaviour
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x000518AA File Offset: 0x0004FAAA
		// (set) Token: 0x0600161D RID: 5661 RVA: 0x000518B2 File Offset: 0x0004FAB2
		public RectTransform rectTransform { get; private set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x000518BB File Offset: 0x0004FABB
		public NavEntry NavEntry
		{
			get
			{
				return this.navEntry;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x000518C3 File Offset: 0x0004FAC3
		// (set) Token: 0x06001620 RID: 5664 RVA: 0x000518CB File Offset: 0x0004FACB
		public GoldMinerArtifact Target { get; private set; }

		// Token: 0x06001621 RID: 5665 RVA: 0x000518D4 File Offset: 0x0004FAD4
		internal void Setup(GoldMinerArtifact target, int amount)
		{
			this.Target = target;
			this.icon.sprite = target.Icon;
			this.rectTransform = (base.transform as RectTransform);
			this.amounText.text = ((amount > 1) ? string.Format("{0}", amount) : "");
		}

		// Token: 0x04001063 RID: 4195
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x04001064 RID: 4196
		[SerializeField]
		private Image icon;

		// Token: 0x04001065 RID: 4197
		[SerializeField]
		private TextMeshProUGUI amounText;
	}
}
