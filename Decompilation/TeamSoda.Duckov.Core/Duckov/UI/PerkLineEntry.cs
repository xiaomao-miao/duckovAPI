using System;
using Duckov.PerkTrees;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003BB RID: 955
	public class PerkLineEntry : MonoBehaviour
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x00079617 File Offset: 0x00077817
		public RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x00079639 File Offset: 0x00077839
		internal void Setup(PerkTreeView perkTreeView, PerkLevelLineNode cur)
		{
			this.target = cur;
			this.label.text = this.target.DisplayName;
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x00079658 File Offset: 0x00077858
		internal Vector2 GetLayoutPosition()
		{
			if (this.target == null)
			{
				return Vector2.zero;
			}
			return this.target.cachedPosition;
		}

		// Token: 0x040017A0 RID: 6048
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x040017A1 RID: 6049
		private RectTransform _rectTransform;

		// Token: 0x040017A2 RID: 6050
		private PerkLevelLineNode target;
	}
}
