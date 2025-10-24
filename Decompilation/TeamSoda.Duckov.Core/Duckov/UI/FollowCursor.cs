using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003C2 RID: 962
	public class FollowCursor : MonoBehaviour
	{
		// Token: 0x0600230E RID: 8974 RVA: 0x0007AB8B File Offset: 0x00078D8B
		private void Awake()
		{
			this.parentRectTransform = (base.transform.parent as RectTransform);
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0007ABB4 File Offset: 0x00078DB4
		private unsafe void Update()
		{
			Vector2 screenPoint = *Mouse.current.position.value;
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, screenPoint, null, out v);
			this.rectTransform.localPosition = v;
		}

		// Token: 0x040017D3 RID: 6099
		private RectTransform parentRectTransform;

		// Token: 0x040017D4 RID: 6100
		private RectTransform rectTransform;
	}
}
