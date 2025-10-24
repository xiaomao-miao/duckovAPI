using System;
using Duckov;
using NodeCanvas.Framework;
using SodaCraft.Localizations;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200041B RID: 1051
	public class TryToReloadIfEmpty : ActionTask<AICharacterController>
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x00082246 File Offset: 0x00080446
		public string SoundKey
		{
			get
			{
				return "normal";
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x0008224D File Offset: 0x0008044D
		private string Key
		{
			get
			{
				return this.poptextWhileReloading;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x00082255 File Offset: 0x00080455
		private string DisplayText
		{
			get
			{
				return this.poptextWhileReloading.ToPlainText();
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00082262 File Offset: 0x00080462
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00082268 File Offset: 0x00080468
		protected override void OnExecute()
		{
			ItemAgent_Gun gun = base.agent.CharacterMainControl.GetGun();
			if (gun == null)
			{
				base.EndAction(true);
				return;
			}
			if (gun.BulletCount <= 0)
			{
				base.agent.CharacterMainControl.TryToReload(null);
				if (!this.isFirstTime)
				{
					if (!base.agent.CharacterMainControl.Health.Hidden && this.poptextWhileReloading != string.Empty && base.agent.canTalk)
					{
						base.agent.CharacterMainControl.PopText(this.poptextWhileReloading.ToPlainText(), -1f);
					}
					if (this.postSound && this.SoundKey != string.Empty && base.agent && base.agent.CharacterMainControl)
					{
						AudioManager.PostQuak(this.SoundKey, base.agent.CharacterMainControl.AudioVoiceType, base.agent.CharacterMainControl.gameObject);
					}
				}
			}
			this.isFirstTime = false;
			base.EndAction(true);
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00082389 File Offset: 0x00080589
		protected override void OnUpdate()
		{
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x0008238B File Offset: 0x0008058B
		protected override void OnStop()
		{
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x0008238D File Offset: 0x0008058D
		protected override void OnPause()
		{
		}

		// Token: 0x040019B4 RID: 6580
		public string poptextWhileReloading = "PopText_Reloading";

		// Token: 0x040019B5 RID: 6581
		public bool postSound;

		// Token: 0x040019B6 RID: 6582
		private bool isFirstTime = true;
	}
}
