using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x020002FD RID: 765
	public class EmptyEntry : MonoBehaviour
	{
		// Token: 0x060018E5 RID: 6373 RVA: 0x0005AAD8 File Offset: 0x00058CD8
		public void Setup(params string[] args)
		{
			this.layoutElement.preferredWidth = this.defaultWidth;
			this.layoutElement.preferredHeight = this.defaultHeight;
			if (args == null)
			{
				return;
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (i == 1)
				{
					this.TrySetWidth(args[i]);
				}
				if (i == 2)
				{
					this.TrySetHeight(args[i]);
				}
			}
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x0005AB34 File Offset: 0x00058D34
		private void TrySetWidth(string v)
		{
			float preferredWidth;
			if (!float.TryParse(v, out preferredWidth))
			{
				return;
			}
			this.layoutElement.preferredWidth = preferredWidth;
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0005AB58 File Offset: 0x00058D58
		private void TrySetHeight(string v)
		{
			float preferredHeight;
			if (!float.TryParse(v, out preferredHeight))
			{
				return;
			}
			this.layoutElement.preferredHeight = preferredHeight;
		}

		// Token: 0x04001219 RID: 4633
		[SerializeField]
		private LayoutElement layoutElement;

		// Token: 0x0400121A RID: 4634
		[SerializeField]
		private float defaultWidth;

		// Token: 0x0400121B RID: 4635
		[SerializeField]
		private float defaultHeight;
	}
}
