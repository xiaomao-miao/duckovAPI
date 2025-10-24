using System;

namespace Duckov.UI
{
	// Token: 0x0200039D RID: 925
	public struct SlotDisplayOperationContext
	{
		// Token: 0x06002106 RID: 8454 RVA: 0x000735C0 File Offset: 0x000717C0
		public SlotDisplayOperationContext(SlotDisplay slotDisplay, SlotDisplayOperationContext.Operation operation, bool succeed)
		{
			this.slotDisplay = slotDisplay;
			this.operation = operation;
			this.succeed = succeed;
		}

		// Token: 0x0400166C RID: 5740
		public SlotDisplay slotDisplay;

		// Token: 0x0400166D RID: 5741
		public SlotDisplayOperationContext.Operation operation;

		// Token: 0x0400166E RID: 5742
		public bool succeed;

		// Token: 0x0200061D RID: 1565
		public enum Operation
		{
			// Token: 0x040021AF RID: 8623
			None,
			// Token: 0x040021B0 RID: 8624
			Equip,
			// Token: 0x040021B1 RID: 8625
			Unequip,
			// Token: 0x040021B2 RID: 8626
			Deny
		}
	}
}
