using System;
using TMPro;
using UnityEngine;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000300 RID: 768
	public class TextEntry : MonoBehaviour
	{
		// Token: 0x060018ED RID: 6381 RVA: 0x0005AC30 File Offset: 0x00058E30
		internal void Setup(string text, Color color, int size = -1, bool bold = false)
		{
			this.text.text = text;
			if (size < 0)
			{
				size = this.defaultSize;
			}
			this.text.color = color;
			this.text.fontSize = (float)size;
			this.text.fontStyle = ((this.text.fontStyle & ~FontStyles.Bold) | (bold ? FontStyles.Bold : FontStyles.Normal));
		}

		// Token: 0x0400121E RID: 4638
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400121F RID: 4639
		public int defaultSize = 26;
	}
}
