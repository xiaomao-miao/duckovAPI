using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040B RID: 1035
	public class FindMainCharacter : ActionTask<AICharacterController>
	{
		// Token: 0x06002567 RID: 9575 RVA: 0x00080E18 File Offset: 0x0007F018
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x00080E1B File Offset: 0x0007F01B
		protected override void OnExecute()
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.mainCharacter.value = LevelManager.Instance.MainCharacter;
			if (this.mainCharacter.value != null)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x00080E5A File Offset: 0x0007F05A
		protected override void OnUpdate()
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.mainCharacter.value = LevelManager.Instance.MainCharacter;
			if (this.mainCharacter.value != null)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x00080E99 File Offset: 0x0007F099
		protected override void OnStop()
		{
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x00080E9B File Offset: 0x0007F09B
		protected override void OnPause()
		{
		}

		// Token: 0x04001971 RID: 6513
		public BBParameter<CharacterMainControl> mainCharacter;
	}
}
