using System;
using UnityEngine.Events;

namespace Duckov.MiniGames
{
	// Token: 0x0200027E RID: 638
	public class VirtualCursorTarget : MiniGameBehaviour
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0004B411 File Offset: 0x00049611
		public bool IsHovering
		{
			get
			{
				return VirtualCursor.IsHovering(this);
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0004B419 File Offset: 0x00049619
		public void OnCursorEnter()
		{
			UnityEvent unityEvent = this.onEnter;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0004B42B File Offset: 0x0004962B
		public void OnCursorExit()
		{
			UnityEvent unityEvent = this.onExit;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0004B43D File Offset: 0x0004963D
		public void OnClick()
		{
			UnityEvent unityEvent = this.onClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000EE2 RID: 3810
		public UnityEvent onEnter;

		// Token: 0x04000EE3 RID: 3811
		public UnityEvent onExit;

		// Token: 0x04000EE4 RID: 3812
		public UnityEvent onClick;
	}
}
