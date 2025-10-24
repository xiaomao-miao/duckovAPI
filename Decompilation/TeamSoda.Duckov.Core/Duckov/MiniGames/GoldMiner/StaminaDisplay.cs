using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AA RID: 682
	public class StaminaDisplay : MonoBehaviour
	{
		// Token: 0x06001639 RID: 5689 RVA: 0x00051F34 File Offset: 0x00050134
		private void FixedUpdate()
		{
			this.Refresh();
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00051F3C File Offset: 0x0005013C
		private void Refresh()
		{
			if (this.master == null)
			{
				return;
			}
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			float stamina = run.stamina;
			float value = run.maxStamina.Value;
			float value2 = run.extraStamina.Value;
			if (stamina > 0f)
			{
				float num = stamina / value;
				this.fill.fillAmount = num;
				this.fill.color = this.normalColor.Evaluate(num);
				this.text.text = string.Format("{0:0.0}", stamina);
				return;
			}
			float num2 = value2 + stamina;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			float fillAmount = num2 / value2;
			this.fill.fillAmount = fillAmount;
			this.fill.color = this.extraColor;
			this.text.text = string.Format("{0:0.00}", num2);
		}

		// Token: 0x04001075 RID: 4213
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001076 RID: 4214
		[SerializeField]
		private Image fill;

		// Token: 0x04001077 RID: 4215
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001078 RID: 4216
		[SerializeField]
		private Gradient normalColor;

		// Token: 0x04001079 RID: 4217
		[SerializeField]
		private Color extraColor = Color.red;
	}
}
