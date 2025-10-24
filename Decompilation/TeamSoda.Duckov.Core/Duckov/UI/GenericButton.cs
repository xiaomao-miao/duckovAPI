using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x0200038D RID: 909
	public class GenericButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x06001FA5 RID: 8101 RVA: 0x0006E9AC File Offset: 0x0006CBAC
		public void OnPointerClick(PointerEventData eventData)
		{
			UnityEvent unityEvent = this.onPointerClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0006E9C0 File Offset: 0x0006CBC0
		public void OnPointerDown(PointerEventData eventData)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggleAnimations)
			{
				toggleAnimation.SetToggle(true);
			}
			UnityEvent unityEvent = this.onPointerDown;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0006EA24 File Offset: 0x0006CC24
		public void OnPointerUp(PointerEventData eventData)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggleAnimations)
			{
				toggleAnimation.SetToggle(false);
			}
			UnityEvent unityEvent = this.onPointerUp;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0400159E RID: 5534
		public List<ToggleAnimation> toggleAnimations = new List<ToggleAnimation>();

		// Token: 0x0400159F RID: 5535
		public UnityEvent onPointerClick;

		// Token: 0x040015A0 RID: 5536
		public UnityEvent onPointerDown;

		// Token: 0x040015A1 RID: 5537
		public UnityEvent onPointerUp;
	}
}
