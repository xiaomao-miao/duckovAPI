using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.HelloWorld
{
	// Token: 0x020002CE RID: 718
	public class FakeMouse : MiniGameBehaviour
	{
		// Token: 0x060016B2 RID: 5810 RVA: 0x000531A5 File Offset: 0x000513A5
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.parentRectTransform = (base.transform.parent as RectTransform);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000531D0 File Offset: 0x000513D0
		protected override void OnUpdate(float deltaTime)
		{
			Vector3 vector = this.rectTransform.localPosition;
			vector += base.Game.GetAxis(1) * this.sensitivity;
			Rect rect = this.parentRectTransform.rect;
			vector.x = Mathf.Clamp(vector.x, rect.xMin, rect.xMax);
			vector.y = Mathf.Clamp(vector.y, rect.yMin, rect.yMax);
			this.rectTransform.localPosition = vector;
		}

		// Token: 0x04001090 RID: 4240
		[SerializeField]
		private float sensitivity = 1f;

		// Token: 0x04001091 RID: 4241
		private RectTransform rectTransform;

		// Token: 0x04001092 RID: 4242
		private RectTransform parentRectTransform;
	}
}
