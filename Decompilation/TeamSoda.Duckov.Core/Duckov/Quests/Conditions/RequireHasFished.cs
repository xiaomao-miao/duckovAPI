using System;
using Saves;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000364 RID: 868
	public class RequireHasFished : Condition
	{
		// Token: 0x06001E47 RID: 7751 RVA: 0x0006AA4E File Offset: 0x00068C4E
		public override bool Evaluate()
		{
			return RequireHasFished.GetHasFished();
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0006AA55 File Offset: 0x00068C55
		public static void SetHasFished()
		{
			SavesSystem.Save<bool>("HasFished", true);
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0006AA62 File Offset: 0x00068C62
		public static bool GetHasFished()
		{
			return SavesSystem.Load<bool>("HasFished");
		}
	}
}
