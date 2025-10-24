using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000301 RID: 769
	public class VerticalEntry : MonoBehaviour
	{
		// Token: 0x060018EF RID: 6383 RVA: 0x0005ACA0 File Offset: 0x00058EA0
		public void Setup(params string[] args)
		{
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0005ACA2 File Offset: 0x00058EA2
		public void SetLayoutSpacing(float spacing)
		{
			this.layoutGroup.spacing = spacing;
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0005ACB0 File Offset: 0x00058EB0
		public void SetPreferredWidth(float width)
		{
			this.layoutElement.preferredWidth = width;
		}

		// Token: 0x04001220 RID: 4640
		[SerializeField]
		private VerticalLayoutGroup layoutGroup;

		// Token: 0x04001221 RID: 4641
		[SerializeField]
		private LayoutElement layoutElement;
	}
}
