using System;

namespace ItemStatsSystem
{
	// Token: 0x02000019 RID: 25
	[MenuPath("General/Update")]
	public class UpdateTrigger : EffectTrigger
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004020 File Offset: 0x00002220
		public override string DisplayName
		{
			get
			{
				return "Update";
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004027 File Offset: 0x00002227
		private void Update()
		{
			base.Trigger(true);
		}
	}
}
