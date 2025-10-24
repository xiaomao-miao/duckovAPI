using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200024E RID: 590
	public class AddPlayerStorage : PerkBehaviour
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00045B69 File Offset: 0x00043D69
		private string DescriptionFormat
		{
			get
			{
				return "PerkBehaviour_AddPlayerStorage".ToPlainText();
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x00045B75 File Offset: 0x00043D75
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.addCapacity
				});
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00045B8D File Offset: 0x00043D8D
		protected override void OnAwake()
		{
			PlayerStorage.OnRecalculateStorageCapacity += this.OnRecalculatePlayerStorage;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00045BA0 File Offset: 0x00043DA0
		protected override void OnOnDestroy()
		{
			PlayerStorage.OnRecalculateStorageCapacity -= this.OnRecalculatePlayerStorage;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00045BB3 File Offset: 0x00043DB3
		private void OnRecalculatePlayerStorage(PlayerStorage.StorageCapacityCalculationHolder holder)
		{
			if (base.Master.Unlocked)
			{
				holder.capacity += this.addCapacity;
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00045BD5 File Offset: 0x00043DD5
		protected override void OnUnlocked()
		{
			base.OnUnlocked();
			PlayerStorage.NotifyCapacityDirty();
		}

		// Token: 0x04000E0E RID: 3598
		[SerializeField]
		private int addCapacity;
	}
}
