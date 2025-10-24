using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A0 RID: 928
	public class TagsDisplayEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITooltipsProvider
	{
		// Token: 0x06002118 RID: 8472 RVA: 0x000737C9 File Offset: 0x000719C9
		public string GetTooltipsText()
		{
			if (this.target == null)
			{
				return "";
			}
			return this.target.Description;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000737EA File Offset: 0x000719EA
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.target == null)
			{
				return;
			}
			if (!this.target.ShowDescription)
			{
				return;
			}
			Tooltips.NotifyEnterTooltipsProvider(this);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0007380F File Offset: 0x00071A0F
		public void OnPointerExit(PointerEventData eventData)
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x00073817 File Offset: 0x00071A17
		private void OnDisable()
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0007381F File Offset: 0x00071A1F
		public void Setup(Tag tag)
		{
			this.target = tag;
			this.background.color = tag.Color;
			this.text.text = tag.DisplayName;
		}

		// Token: 0x04001673 RID: 5747
		[SerializeField]
		private Image background;

		// Token: 0x04001674 RID: 5748
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001675 RID: 5749
		private Tag target;
	}
}
