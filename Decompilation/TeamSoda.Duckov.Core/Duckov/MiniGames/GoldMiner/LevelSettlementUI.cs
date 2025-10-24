using System;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A4 RID: 676
	public class LevelSettlementUI : MonoBehaviour
	{
		// Token: 0x060015FE RID: 5630 RVA: 0x00051439 File Offset: 0x0004F639
		internal void Reset()
		{
			this.clearIndicator.SetActive(false);
			this.failIndicator.SetActive(false);
			this.money = 0;
			this.score = 0;
			this.factor = 0f;
			this.RefreshTexts();
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00051472 File Offset: 0x0004F672
		public void SetTargetScore(int targetScore)
		{
			this.targetScore = targetScore;
			this.RefreshTexts();
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00051481 File Offset: 0x0004F681
		public void StepResolveEntity(GoldMinerEntity entity)
		{
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00051483 File Offset: 0x0004F683
		public void StepResult(bool clear)
		{
			this.clearIndicator.SetActive(clear);
			this.failIndicator.SetActive(!clear);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x000514A0 File Offset: 0x0004F6A0
		public void Step(int money, float factor, int score)
		{
			bool flag = money > this.money;
			bool flag2 = factor > this.factor;
			bool flag3 = score > this.score;
			this.money = money;
			this.factor = factor;
			this.score = score;
			this.RefreshTexts();
			if (flag)
			{
				this.moneyPunch.Punch();
			}
			if (flag2)
			{
				this.factorPunch.Punch();
			}
			if (flag3)
			{
				this.scorePunch.Punch();
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00051510 File Offset: 0x0004F710
		private void RefreshTexts()
		{
			this.levelText.text = string.Format("LEVEL {0}", this.goldMiner.run.level + 1);
			this.targetScoreText.text = string.Format("{0}", this.targetScore);
			this.moneyText.text = string.Format("${0}", this.money);
			this.factorText.text = string.Format("{0}", this.factor);
			this.scoreText.text = string.Format("{0}", this.score);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000515C9 File Offset: 0x0004F7C9
		public void Show()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x000515D6 File Offset: 0x0004F7D6
		public void Hide()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x04001048 RID: 4168
		[SerializeField]
		private GoldMiner goldMiner;

		// Token: 0x04001049 RID: 4169
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400104A RID: 4170
		[SerializeField]
		private PunchReceiver moneyPunch;

		// Token: 0x0400104B RID: 4171
		[SerializeField]
		private PunchReceiver factorPunch;

		// Token: 0x0400104C RID: 4172
		[SerializeField]
		private PunchReceiver scorePunch;

		// Token: 0x0400104D RID: 4173
		[SerializeField]
		private TextMeshProUGUI moneyText;

		// Token: 0x0400104E RID: 4174
		[SerializeField]
		private TextMeshProUGUI factorText;

		// Token: 0x0400104F RID: 4175
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04001050 RID: 4176
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x04001051 RID: 4177
		[SerializeField]
		private TextMeshProUGUI targetScoreText;

		// Token: 0x04001052 RID: 4178
		[SerializeField]
		private GameObject clearIndicator;

		// Token: 0x04001053 RID: 4179
		[SerializeField]
		private GameObject failIndicator;

		// Token: 0x04001054 RID: 4180
		private int targetScore;

		// Token: 0x04001055 RID: 4181
		private int money;

		// Token: 0x04001056 RID: 4182
		private int score;

		// Token: 0x04001057 RID: 4183
		private float factor;
	}
}
