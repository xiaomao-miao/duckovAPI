using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BF RID: 959
	public class CloseViewOnPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060022E0 RID: 8928 RVA: 0x0007A2D8 File Offset: 0x000784D8
		private void OnValidate()
		{
			if (this.view == null)
			{
				this.view = base.GetComponent<View>();
			}
			if (this.graphic == null)
			{
				this.graphic = base.GetComponent<Graphic>();
			}
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0007A310 File Offset: 0x00078510
		private void Awake()
		{
			if (this.view == null)
			{
				this.view = base.GetComponent<View>();
			}
			if (this.graphic == null)
			{
				this.graphic = base.GetComponent<Graphic>();
			}
			ManagedUIElement.onOpen += this.OnViewOpen;
			ManagedUIElement.onClose += this.OnViewClose;
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x0007A373 File Offset: 0x00078573
		private void OnDestroy()
		{
			ManagedUIElement.onOpen -= this.OnViewOpen;
			ManagedUIElement.onClose -= this.OnViewClose;
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0007A397 File Offset: 0x00078597
		private void OnViewClose(ManagedUIElement element)
		{
			if (element != this.view)
			{
				return;
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.enabled = false;
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0007A3C3 File Offset: 0x000785C3
		private void OnViewOpen(ManagedUIElement element)
		{
			if (element != this.view)
			{
				return;
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.enabled = true;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0007A3F0 File Offset: 0x000785F0
		public void OnPointerClick(PointerEventData eventData)
		{
		}

		// Token: 0x040017B7 RID: 6071
		private const bool FunctionEnabled = false;

		// Token: 0x040017B8 RID: 6072
		[SerializeField]
		private View view;

		// Token: 0x040017B9 RID: 6073
		[SerializeField]
		private Graphic graphic;
	}
}
