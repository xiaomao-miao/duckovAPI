using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000381 RID: 897
	public class TooltipsProvider : MonoBehaviour, ITooltipsProvider, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06001F1A RID: 7962 RVA: 0x0006D027 File Offset: 0x0006B227
		public string GetTooltipsText()
		{
			return this.text;
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0006D02F File Offset: 0x0006B22F
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return;
			}
			Tooltips.NotifyEnterTooltipsProvider(this);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0006D045 File Offset: 0x0006B245
		public void OnPointerExit(PointerEventData eventData)
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0006D04D File Offset: 0x0006B24D
		private void OnDisable()
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x04001544 RID: 5444
		public string text;
	}
}
