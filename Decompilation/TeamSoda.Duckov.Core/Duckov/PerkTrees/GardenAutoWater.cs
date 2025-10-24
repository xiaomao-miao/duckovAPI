using System;
using Duckov.Crops;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024F RID: 591
	public class GardenAutoWater : PerkBehaviour, IGardenAutoWaterProvider
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x00045BEA File Offset: 0x00043DEA
		public override string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00045BF7 File Offset: 0x00043DF7
		protected override void OnUnlocked()
		{
			Garden.Register(this);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00045BFF File Offset: 0x00043DFF
		protected override void OnOnDestroy()
		{
			Garden.Unregister(this);
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00045C07 File Offset: 0x00043E07
		public bool TakeEffect(string gardenID)
		{
			return gardenID == this.gardenID;
		}

		// Token: 0x04000E0F RID: 3599
		[SerializeField]
		[LocalizationKey("Default")]
		private string descriptionKey = "PerkBehaviour_GardenAutoWater";

		// Token: 0x04000E10 RID: 3600
		[SerializeField]
		private string gardenID = "Default";
	}
}
