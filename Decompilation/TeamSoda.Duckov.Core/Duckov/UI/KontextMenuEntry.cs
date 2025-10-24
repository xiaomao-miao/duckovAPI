using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C8 RID: 968
	public class KontextMenuEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600233F RID: 9023 RVA: 0x0007B6DF File Offset: 0x000798DF
		public void NotifyPooled()
		{
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0007B6E1 File Offset: 0x000798E1
		public void NotifyReleased()
		{
			this.target = null;
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0007B6EA File Offset: 0x000798EA
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.menu != null)
			{
				this.menu.InstanceHide();
			}
			if (this.target != null)
			{
				Action action = this.target.action;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0007B724 File Offset: 0x00079924
		public void Setup(KontextMenu menu, int index, KontextMenuDataEntry data)
		{
			this.menu = menu;
			this.target = data;
			if (this.icon)
			{
				if (data.icon)
				{
					this.icon.sprite = data.icon;
					this.icon.gameObject.SetActive(true);
				}
				else
				{
					this.icon.gameObject.SetActive(false);
				}
			}
			if (this.text)
			{
				if (!string.IsNullOrEmpty(this.target.text))
				{
					this.text.text = this.target.text;
					this.text.gameObject.SetActive(true);
				}
				else
				{
					this.text.gameObject.SetActive(false);
				}
			}
			foreach (FadeElement fadeElement in this.fadeInElements)
			{
				fadeElement.SkipHide();
				fadeElement.Show(this.delayByIndex * (float)index).Forget();
			}
		}

		// Token: 0x040017F4 RID: 6132
		[SerializeField]
		private Image icon;

		// Token: 0x040017F5 RID: 6133
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040017F6 RID: 6134
		[SerializeField]
		private float delayByIndex = 0.1f;

		// Token: 0x040017F7 RID: 6135
		[SerializeField]
		private List<FadeElement> fadeInElements;

		// Token: 0x040017F8 RID: 6136
		private KontextMenu menu;

		// Token: 0x040017F9 RID: 6137
		private KontextMenuDataEntry target;
	}
}
