using System;

namespace ItemStatsSystem
{
	// Token: 0x02000014 RID: 20
	[MenuPath("Debug/Bool")]
	public class BoolFilter : EffectFilter
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003DFF File Offset: 0x00001FFF
		public override string DisplayName
		{
			get
			{
				return "根据 Bool 值";
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003E06 File Offset: 0x00002006
		protected override bool OnEvaluate(EffectTriggerEventContext context)
		{
			return this.value;
		}

		// Token: 0x0400003A RID: 58
		public bool value;
	}
}
