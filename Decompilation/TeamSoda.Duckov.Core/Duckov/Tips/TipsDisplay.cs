using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Duckov.Tips
{
	// Token: 0x02000246 RID: 582
	public class TipsDisplay : MonoBehaviour
	{
		// Token: 0x0600121A RID: 4634 RVA: 0x00044F50 File Offset: 0x00043150
		public void DisplayRandom()
		{
			if (this.entries.Length == 0)
			{
				return;
			}
			TipEntry tipEntry = this.entries[UnityEngine.Random.Range(0, this.entries.Length)];
			this.text.text = tipEntry.Description;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00044F94 File Offset: 0x00043194
		public void Display(string tipID)
		{
			TipEntry tipEntry = this.entries.FirstOrDefault((TipEntry e) => e.TipID == tipID);
			if (tipEntry.TipID != tipID)
			{
				return;
			}
			this.text.text = tipEntry.Description;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00044FED File Offset: 0x000431ED
		private void OnEnable()
		{
			this.canvasGroup.alpha = (SceneLoader.HideTips ? 0f : 1f);
			this.DisplayRandom();
		}

		// Token: 0x04000DE8 RID: 3560
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000DE9 RID: 3561
		[SerializeField]
		private TipEntry[] entries;

		// Token: 0x04000DEA RID: 3562
		[SerializeField]
		private CanvasGroup canvasGroup;
	}
}
