using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A9 RID: 681
	public class ScoreDisplay : MonoBehaviour
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x00051DAC File Offset: 0x0004FFAC
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = this.master;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00051E07 File Offset: 0x00050007
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			this.Refresh();
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00051E0F File Offset: 0x0005000F
		private void OnLevelBegin(GoldMiner miner)
		{
			this.Refresh();
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00051E18 File Offset: 0x00050018
		private void Refresh()
		{
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			int num = 0;
			float num2 = run.scoreFactorBase.Value + run.levelScoreFactor;
			int targetScore = run.targetScore;
			foreach (GoldMinerEntity goldMinerEntity in this.master.resolvedEntities)
			{
				int num3 = Mathf.CeilToInt((float)goldMinerEntity.Value * run.charm.Value);
				if (num3 != 0)
				{
					num += num3;
				}
			}
			this.moneyText.text = string.Format("${0}", num);
			this.factorText.text = string.Format("{0}", num2);
			this.scoreText.text = string.Format("{0}", Mathf.CeilToInt((float)num * num2));
			this.targetScoreText.text = string.Format("{0}", targetScore);
		}

		// Token: 0x04001070 RID: 4208
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001071 RID: 4209
		[SerializeField]
		private TextMeshProUGUI moneyText;

		// Token: 0x04001072 RID: 4210
		[SerializeField]
		private TextMeshProUGUI factorText;

		// Token: 0x04001073 RID: 4211
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04001074 RID: 4212
		[SerializeField]
		private TextMeshProUGUI targetScoreText;
	}
}
