using System;

namespace ItemStatsSystem
{
	// Token: 0x02000018 RID: 24
	public class TriggerOnSetItem : EffectTrigger
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x0000400F File Offset: 0x0000220F
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			base.Trigger(true);
		}
	}
}
