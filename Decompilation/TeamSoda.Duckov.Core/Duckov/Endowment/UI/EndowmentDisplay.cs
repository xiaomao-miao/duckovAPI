using System;
using TMPro;
using UnityEngine;

namespace Duckov.Endowment.UI
{
	// Token: 0x020002F6 RID: 758
	public class EndowmentDisplay : MonoBehaviour
	{
		// Token: 0x060018A6 RID: 6310 RVA: 0x00059BDC File Offset: 0x00057DDC
		private void Refresh()
		{
			EndowmentEntry endowmentEntry = EndowmentManager.Current;
			if (endowmentEntry == null)
			{
				this.displayNameText.text = "?";
				this.descriptionsText.text = "?";
				return;
			}
			this.displayNameText.text = endowmentEntry.DisplayName;
			this.descriptionsText.text = endowmentEntry.DescriptionAndEffects;
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00059C3B File Offset: 0x00057E3B
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x040011EE RID: 4590
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040011EF RID: 4591
		[SerializeField]
		private TextMeshProUGUI descriptionsText;
	}
}
