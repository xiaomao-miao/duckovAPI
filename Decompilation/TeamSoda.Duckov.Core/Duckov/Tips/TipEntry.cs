using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Tips
{
	// Token: 0x02000247 RID: 583
	[Serializable]
	internal struct TipEntry
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x0004501B File Offset: 0x0004321B
		public string TipID
		{
			get
			{
				return this.tipID;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00045023 File Offset: 0x00043223
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x00045035 File Offset: 0x00043235
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Tips_" + this.tipID;
			}
			set
			{
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00045037 File Offset: 0x00043237
		public string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x04000DEB RID: 3563
		[SerializeField]
		private string tipID;
	}
}
