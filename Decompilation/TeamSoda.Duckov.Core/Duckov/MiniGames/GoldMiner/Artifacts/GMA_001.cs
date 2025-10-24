using System;
using ItemStatsSystem.Stats;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002AD RID: 685
	public class GMA_001 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001648 RID: 5704 RVA: 0x000521EC File Offset: 0x000503EC
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			this.cachedRun = base.Run;
			this.staminaModifier = new Modifier(ModifierType.Add, 1f, this);
			this.scoreFactorModifier = new Modifier(ModifierType.Add, 1f, this);
			this.cachedRun.staminaDrain.AddModifier(this.staminaModifier);
			this.cachedRun.scoreFactorBase.AddModifier(this.scoreFactorModifier);
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0005225E File Offset: 0x0005045E
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (this.cachedRun == null)
			{
				return;
			}
			this.cachedRun.staminaDrain.RemoveModifier(this.staminaModifier);
			this.cachedRun.scoreFactorBase.RemoveModifier(this.scoreFactorModifier);
		}

		// Token: 0x04001083 RID: 4227
		private Modifier staminaModifier;

		// Token: 0x04001084 RID: 4228
		private Modifier scoreFactorModifier;

		// Token: 0x04001085 RID: 4229
		private GoldMinerRunData cachedRun;
	}
}
