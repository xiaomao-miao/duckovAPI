using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D0 RID: 720
	public class UICrossHair : MiniGameBehaviour
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x00053368 File Offset: 0x00051568
		private float ScatterAngle
		{
			get
			{
				if (this.gunControl)
				{
					return this.gunControl.ScatterAngle;
				}
				return 0f;
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00053388 File Offset: 0x00051588
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000533A4 File Offset: 0x000515A4
		protected override void OnUpdate(float deltaTime)
		{
			float scatterAngle = this.ScatterAngle;
			float fieldOfView = base.Game.Camera.fieldOfView;
			float y = this.canvasRectTransform.sizeDelta.y;
			float num = scatterAngle / fieldOfView;
			float d = (float)(Mathf.FloorToInt(y * num / 2f) * 2 + 1);
			this.rectTransform.sizeDelta = d * Vector2.one;
		}

		// Token: 0x04001097 RID: 4247
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001098 RID: 4248
		[SerializeField]
		private RectTransform canvasRectTransform;

		// Token: 0x04001099 RID: 4249
		[SerializeField]
		private FPSGunControl gunControl;
	}
}
