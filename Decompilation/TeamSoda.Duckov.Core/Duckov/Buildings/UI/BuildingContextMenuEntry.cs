using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031C RID: 796
	public class BuildingContextMenuEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001A79 RID: 6777 RVA: 0x0005FC31 File Offset: 0x0005DE31
		private void OnEnable()
		{
			this.text.text = this.textKey.ToPlainText();
		}

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06001A7A RID: 6778 RVA: 0x0005FC4C File Offset: 0x0005DE4C
		// (remove) Token: 0x06001A7B RID: 6779 RVA: 0x0005FC84 File Offset: 0x0005DE84
		public event Action<BuildingContextMenuEntry> onPointerClick;

		// Token: 0x06001A7C RID: 6780 RVA: 0x0005FCB9 File Offset: 0x0005DEB9
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<BuildingContextMenuEntry> action = this.onPointerClick;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040012FC RID: 4860
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040012FD RID: 4861
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey;
	}
}
