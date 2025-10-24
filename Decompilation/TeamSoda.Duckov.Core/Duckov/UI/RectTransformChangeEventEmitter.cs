using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003C3 RID: 963
	[ExecuteInEditMode]
	public class RectTransformChangeEventEmitter : UIBehaviour
	{
		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06002311 RID: 8977 RVA: 0x0007AC00 File Offset: 0x00078E00
		// (remove) Token: 0x06002312 RID: 8978 RVA: 0x0007AC38 File Offset: 0x00078E38
		public event Action<RectTransform> OnRectTransformChange;

		// Token: 0x06002313 RID: 8979 RVA: 0x0007AC6D File Offset: 0x00078E6D
		private void SetDirty()
		{
			Action<RectTransform> onRectTransformChange = this.OnRectTransformChange;
			if (onRectTransformChange == null)
			{
				return;
			}
			onRectTransformChange(base.transform as RectTransform);
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0007AC8A File Offset: 0x00078E8A
		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0007AC92 File Offset: 0x00078E92
		protected override void OnEnable()
		{
			this.SetDirty();
		}
	}
}
