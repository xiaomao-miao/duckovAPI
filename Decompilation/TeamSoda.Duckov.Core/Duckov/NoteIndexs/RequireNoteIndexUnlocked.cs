using System;
using Duckov.Quests;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000269 RID: 617
	public class RequireNoteIndexUnlocked : Condition
	{
		// Token: 0x0600132A RID: 4906 RVA: 0x00047609 File Offset: 0x00045809
		public override bool Evaluate()
		{
			return NoteIndex.GetNoteUnlocked(this.key);
		}

		// Token: 0x04000E4C RID: 3660
		public string key;
	}
}
