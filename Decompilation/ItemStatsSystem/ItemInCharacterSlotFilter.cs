using System;

namespace ItemStatsSystem
{
	// Token: 0x02000015 RID: 21
	public class ItemInCharacterSlotFilter : EffectFilter
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00003E16 File Offset: 0x00002016
		protected override bool OnEvaluate(EffectTriggerEventContext context)
		{
			return !(base.Master == null) && !(base.Master.Item == null) && base.Master.Item.IsInCharacterSlot();
		}
	}
}
