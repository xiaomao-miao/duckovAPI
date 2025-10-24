using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F2 RID: 754
	public class GardenViewToolButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600187A RID: 6266 RVA: 0x000596D3 File Offset: 0x000578D3
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.SetTool(this.tool);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x000596E6 File Offset: 0x000578E6
		private void Awake()
		{
			this.master.onToolChanged += this.OnToolChanged;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000596FF File Offset: 0x000578FF
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00059707 File Offset: 0x00057907
		private void Refresh()
		{
			this.indicator.SetActive(this.tool == this.master.Tool);
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00059727 File Offset: 0x00057927
		private void OnToolChanged()
		{
			this.Refresh();
		}

		// Token: 0x040011D7 RID: 4567
		[SerializeField]
		private GardenView master;

		// Token: 0x040011D8 RID: 4568
		[SerializeField]
		private GardenView.ToolType tool;

		// Token: 0x040011D9 RID: 4569
		[SerializeField]
		private GameObject indicator;
	}
}
