using System;
using System.Collections.Generic;

namespace Duckov.Quests
{
	// Token: 0x02000332 RID: 818
	public static class ConditionExtensions
	{
		// Token: 0x06001BBC RID: 7100 RVA: 0x00064878 File Offset: 0x00062A78
		public static bool Satisfied(this IEnumerable<Condition> conditions)
		{
			foreach (Condition condition in conditions)
			{
				if (!(condition == null) && !condition.Evaluate())
				{
					return false;
				}
			}
			return true;
		}
	}
}
