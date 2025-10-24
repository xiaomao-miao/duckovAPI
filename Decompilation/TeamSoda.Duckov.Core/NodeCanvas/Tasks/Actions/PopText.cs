using System;
using NodeCanvas.Framework;
using SodaCraft.Localizations;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040E RID: 1038
	public class PopText : ActionTask<AICharacterController>
	{
		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x0008127E File Offset: 0x0007F47E
		private string Key
		{
			get
			{
				return this.content.value;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x0008128B File Offset: 0x0007F48B
		private string DisplayText
		{
			get
			{
				return this.Key.ToPlainText();
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00081298 File Offset: 0x0007F498
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x0008129B File Offset: 0x0007F49B
		protected override string info
		{
			get
			{
				return string.Format("Pop:'{0}'", this.DisplayText);
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000812B0 File Offset: 0x0007F4B0
		protected override void OnExecute()
		{
			if (this.checkHide && base.agent.CharacterMainControl.Hidden)
			{
				base.EndAction(true);
				return;
			}
			if (!base.agent.canTalk)
			{
				base.EndAction(true);
				return;
			}
			base.agent.CharacterMainControl.PopText(this.DisplayText, -1f);
			base.EndAction(true);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00081316 File Offset: 0x0007F516
		protected override void OnStop()
		{
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00081318 File Offset: 0x0007F518
		protected override void OnPause()
		{
		}

		// Token: 0x04001980 RID: 6528
		public BBParameter<string> content;

		// Token: 0x04001981 RID: 6529
		public bool checkHide;
	}
}
