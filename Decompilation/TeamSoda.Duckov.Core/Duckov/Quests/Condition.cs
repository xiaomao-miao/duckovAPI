using System;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000331 RID: 817
	public class Condition : MonoBehaviour
	{
		// Token: 0x06001BB9 RID: 7097 RVA: 0x00064864 File Offset: 0x00062A64
		public virtual bool Evaluate()
		{
			return false;
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x00064867 File Offset: 0x00062A67
		public virtual string DisplayText
		{
			get
			{
				return "";
			}
		}
	}
}
