using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A2 RID: 674
	public class GoldMiner_PopText : MiniGameBehaviour
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x000512F4 File Offset: 0x0004F4F4
		private PrefabPool<GoldMiner_PopTextEntry> TextPool
		{
			get
			{
				if (this._textPool == null)
				{
					this._textPool = new PrefabPool<GoldMiner_PopTextEntry>(this.textTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._textPool;
			}
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0005132D File Offset: 0x0004F52D
		public void Pop(string content, Vector3 position)
		{
			this.TextPool.Get(null).Setup(position, content, new Action<GoldMiner_PopTextEntry>(this.ReleaseEntry));
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0005134E File Offset: 0x0004F54E
		private void ReleaseEntry(GoldMiner_PopTextEntry entry)
		{
			this.TextPool.Release(entry);
		}

		// Token: 0x04001040 RID: 4160
		[SerializeField]
		private GoldMiner_PopTextEntry textTemplate;

		// Token: 0x04001041 RID: 4161
		private PrefabPool<GoldMiner_PopTextEntry> _textPool;
	}
}
