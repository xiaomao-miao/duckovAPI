using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200028B RID: 651
	public class GoldMinerArtifact : MiniGameBehaviour
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0004E121 File Offset: 0x0004C321
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x0004E133 File Offset: 0x0004C333
		[LocalizationKey("Default")]
		private string displayNameKey
		{
			get
			{
				return "GoldMiner_" + this.id;
			}
			set
			{
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x0004E135 File Offset: 0x0004C335
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x0004E14C File Offset: 0x0004C34C
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "GoldMiner_" + this.id + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x0004E14E File Offset: 0x0004C34E
		public bool AllowMultiple
		{
			get
			{
				return this.allowMultiple;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0004E156 File Offset: 0x0004C356
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0004E163 File Offset: 0x0004C363
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0004E170 File Offset: 0x0004C370
		public int Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0004E178 File Offset: 0x0004C378
		public int BasePrice
		{
			get
			{
				return this.basePrice;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0004E180 File Offset: 0x0004C380
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0004E188 File Offset: 0x0004C388
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x0004E190 File Offset: 0x0004C390
		public GoldMiner Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0004E198 File Offset: 0x0004C398
		public void Attach(GoldMiner master)
		{
			this.master = master;
			base.transform.SetParent(master.transform);
			Action<GoldMinerArtifact> onAttached = this.OnAttached;
			if (onAttached == null)
			{
				return;
			}
			onAttached(this);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0004E1C3 File Offset: 0x0004C3C3
		public void Detatch(GoldMiner master)
		{
			Action<GoldMinerArtifact> onDetached = this.OnDetached;
			if (onDetached != null)
			{
				onDetached(this);
			}
			if (master != this.master)
			{
				Debug.LogError("Artifact is being notified detach by a different GoldMiner instance.", master.gameObject);
			}
			this.master = null;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0004E1FC File Offset: 0x0004C3FC
		private void OnDestroy()
		{
			this.Detatch(this.master);
		}

		// Token: 0x04000F71 RID: 3953
		[SerializeField]
		private string id;

		// Token: 0x04000F72 RID: 3954
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000F73 RID: 3955
		[SerializeField]
		private bool allowMultiple;

		// Token: 0x04000F74 RID: 3956
		[SerializeField]
		private int basePrice;

		// Token: 0x04000F75 RID: 3957
		[SerializeField]
		private int quality;

		// Token: 0x04000F76 RID: 3958
		private GoldMiner master;

		// Token: 0x04000F77 RID: 3959
		public Action<GoldMinerArtifact> OnAttached;

		// Token: 0x04000F78 RID: 3960
		public Action<GoldMinerArtifact> OnDetached;
	}
}
