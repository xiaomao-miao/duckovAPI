using System;
using Duckov.UI;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000267 RID: 615
	public class NoteIndexProxy : MonoBehaviour
	{
		// Token: 0x06001322 RID: 4898 RVA: 0x00047546 File Offset: 0x00045746
		public void UnlockNote(string key)
		{
			NoteIndex.SetNoteUnlocked(key);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0004754E File Offset: 0x0004574E
		public void UnlockAndShowNote(string key)
		{
			NoteIndexView.ShowNote(key, true);
		}
	}
}
