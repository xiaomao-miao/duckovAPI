using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.Inventories
{
	// Token: 0x020003CA RID: 970
	public class PagesControl_Entry : MonoBehaviour
	{
		// Token: 0x0600234D RID: 9037 RVA: 0x0007BA01 File Offset: 0x00079C01
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0007BA1F File Offset: 0x00079C1F
		private void OnButtonClicked()
		{
			this.master.NotifySelect(this.index);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0007BA34 File Offset: 0x00079C34
		internal void Setup(PagesControl master, int i, bool selected)
		{
			this.master = master;
			this.index = i;
			this.selected = selected;
			this.text.text = string.Format("{0}", this.index);
			this.selectedIndicator.SetActive(this.selected);
		}

		// Token: 0x040017FE RID: 6142
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040017FF RID: 6143
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001800 RID: 6144
		[SerializeField]
		private Button button;

		// Token: 0x04001801 RID: 6145
		private PagesControl master;

		// Token: 0x04001802 RID: 6146
		private int index;

		// Token: 0x04001803 RID: 6147
		private bool selected;
	}
}
