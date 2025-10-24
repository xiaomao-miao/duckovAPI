using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DF RID: 991
	public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x0007D4C4 File Offset: 0x0007B6C4
		private void Awake()
		{
			this.SetAll(false);
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0007D4E6 File Offset: 0x0007B6E6
		private void OnEnable()
		{
			this.SetAll(false);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0007D4EF File Offset: 0x0007B6EF
		private void OnDisable()
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0007D50C File Offset: 0x0007B70C
		private void SetAll(bool value)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggles)
			{
				if (!(toggleAnimation == null))
				{
					toggleAnimation.SetToggle(value);
				}
			}
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0007D568 File Offset: 0x0007B768
		public void OnPointerDown(PointerEventData eventData)
		{
			this.SetAll(true);
			if (!this.mute)
			{
				AudioManager.Post("UI/click");
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0007D584 File Offset: 0x0007B784
		public void OnPointerUp(PointerEventData eventData)
		{
			this.SetAll(false);
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0007D58D File Offset: 0x0007B78D
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(true);
			}
			if (!this.mute)
			{
				AudioManager.Post("UI/hover");
			}
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0007D5BB File Offset: 0x0007B7BB
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x04001861 RID: 6241
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x04001862 RID: 6242
		[SerializeField]
		private List<ToggleAnimation> toggles = new List<ToggleAnimation>();

		// Token: 0x04001863 RID: 6243
		[SerializeField]
		private bool mute;
	}
}
