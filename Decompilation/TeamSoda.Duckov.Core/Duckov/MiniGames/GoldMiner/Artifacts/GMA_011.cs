using System;

namespace Duckov.MiniGames.GoldMiner.Artifacts
{
	// Token: 0x020002B7 RID: 695
	public class GMA_011 : GoldMinerArtifactBehaviour
	{
		// Token: 0x06001671 RID: 5745 RVA: 0x00052A45 File Offset: 0x00050C45
		protected override void OnAttached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.forceLevelSuccessFuncs.Add(new Func<bool>(this.ForceAndDetach));
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00052A6C File Offset: 0x00050C6C
		private bool ForceAndDetach()
		{
			base.Run.DetachArtifact(this.master);
			return true;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00052A81 File Offset: 0x00050C81
		protected override void OnDetached(GoldMinerArtifact artifact)
		{
			if (base.Run == null)
			{
				return;
			}
			base.Run.forceLevelSuccessFuncs.Remove(new Func<bool>(this.ForceAndDetach));
		}
	}
}
