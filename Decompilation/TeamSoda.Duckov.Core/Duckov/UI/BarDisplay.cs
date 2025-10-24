using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038C RID: 908
	public class BarDisplay : MonoBehaviour
	{
		// Token: 0x06001F9D RID: 8093 RVA: 0x0006E828 File Offset: 0x0006CA28
		private void Awake()
		{
			this.fill.fillAmount = 0f;
			this.ApplyLook();
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0006E840 File Offset: 0x0006CA40
		public void Setup(string labelText, Color color, float current, float max, string format = "0.#", float min = 0f)
		{
			this.SetupLook(labelText, color);
			this.SetValue(current, max, format, min);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0006E857 File Offset: 0x0006CA57
		public void Setup(string labelText, Color color, int current, int max, int min = 0)
		{
			this.SetupLook(labelText, color);
			this.SetValue(current, max, min);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0006E86C File Offset: 0x0006CA6C
		public void SetupLook(string labelText, Color color)
		{
			this.labelText = labelText;
			this.color = color;
			this.ApplyLook();
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0006E882 File Offset: 0x0006CA82
		private void ApplyLook()
		{
			this.text_Label.text = this.labelText.ToPlainText();
			this.fill.color = this.color;
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0006E8AC File Offset: 0x0006CAAC
		public void SetValue(float current, float max, string format = "0.#", float min = 0f)
		{
			this.text_Current.text = current.ToString(format);
			this.text_Max.text = max.ToString(format);
			float num = max - min;
			float endValue = 1f;
			if (num > 0f)
			{
				endValue = (current - min) / num;
			}
			this.fill.DOKill(false);
			this.fill.DOFillAmount(endValue, this.animateDuration).SetEase(Ease.OutCubic);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0006E920 File Offset: 0x0006CB20
		public void SetValue(int current, int max, int min = 0)
		{
			this.text_Current.text = current.ToString();
			this.text_Max.text = max.ToString();
			int num = max - min;
			float endValue = 1f;
			if (num > 0)
			{
				endValue = (float)(current - min) / (float)num;
			}
			this.fill.DOKill(false);
			this.fill.DOFillAmount(endValue, this.animateDuration).SetEase(Ease.OutCubic);
		}

		// Token: 0x04001597 RID: 5527
		[SerializeField]
		private string labelText;

		// Token: 0x04001598 RID: 5528
		[SerializeField]
		private Color color = Color.red;

		// Token: 0x04001599 RID: 5529
		[SerializeField]
		private float animateDuration = 0.25f;

		// Token: 0x0400159A RID: 5530
		[SerializeField]
		private TextMeshProUGUI text_Label;

		// Token: 0x0400159B RID: 5531
		[SerializeField]
		private TextMeshProUGUI text_Current;

		// Token: 0x0400159C RID: 5532
		[SerializeField]
		private TextMeshProUGUI text_Max;

		// Token: 0x0400159D RID: 5533
		[SerializeField]
		private Image fill;
	}
}
