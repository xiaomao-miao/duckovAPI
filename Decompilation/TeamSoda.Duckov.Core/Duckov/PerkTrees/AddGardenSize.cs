using System;
using Duckov.Crops;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024D RID: 589
	public class AddGardenSize : PerkBehaviour, IGardenSizeAdder
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x00045AE3 File Offset: 0x00043CE3
		public override string Description
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText().Format(new
				{
					addX = this.add.x,
					addY = this.add.y
				});
			}
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00045B10 File Offset: 0x00043D10
		protected override void OnUnlocked()
		{
			Garden.Register(this);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00045B18 File Offset: 0x00043D18
		protected override void OnOnDestroy()
		{
			Garden.Unregister(this);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00045B20 File Offset: 0x00043D20
		public Vector2Int GetValue(string gardenID)
		{
			if (gardenID != this.gardenID)
			{
				return default(Vector2Int);
			}
			return this.add;
		}

		// Token: 0x04000E0B RID: 3595
		[LocalizationKey("Default")]
		[SerializeField]
		private string descriptionFormatKey = "PerkBehaviour_AddGardenSize";

		// Token: 0x04000E0C RID: 3596
		[SerializeField]
		private string gardenID = "Default";

		// Token: 0x04000E0D RID: 3597
		[SerializeField]
		private Vector2Int add;
	}
}
