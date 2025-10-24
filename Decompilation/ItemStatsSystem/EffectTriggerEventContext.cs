using System;

namespace ItemStatsSystem
{
	// Token: 0x02000013 RID: 19
	public struct EffectTriggerEventContext
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00003DEF File Offset: 0x00001FEF
		public EffectTriggerEventContext(EffectTrigger source, bool positive)
		{
			this.source = source;
			this.positive = positive;
		}

		// Token: 0x04000038 RID: 56
		public EffectTrigger source;

		// Token: 0x04000039 RID: 57
		public bool positive;
	}
}
