using System;
using Duckov;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200040F RID: 1039
	public class PostSound : ActionTask<AICharacterController>
	{
		// Token: 0x06002580 RID: 9600 RVA: 0x00081322 File Offset: 0x0007F522
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x00081325 File Offset: 0x0007F525
		protected override string info
		{
			get
			{
				return string.Format("Post Sound: {0} ", this.voiceSound.ToString());
			}
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00081344 File Offset: 0x0007F544
		protected override void OnExecute()
		{
			if (base.agent && base.agent.CharacterMainControl)
			{
				if (!base.agent.canTalk)
				{
					base.EndAction(true);
					return;
				}
				GameObject gameObject = base.agent.CharacterMainControl.gameObject;
				switch (this.voiceSound)
				{
				case PostSound.VoiceSounds.normal:
					AudioManager.PostQuak("normal", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				case PostSound.VoiceSounds.surprise:
					AudioManager.PostQuak("surprise", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				case PostSound.VoiceSounds.death:
					AudioManager.PostQuak("death", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			base.EndAction(true);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x0008141E File Offset: 0x0007F61E
		protected override void OnStop()
		{
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00081420 File Offset: 0x0007F620
		protected override void OnPause()
		{
		}

		// Token: 0x04001982 RID: 6530
		public PostSound.VoiceSounds voiceSound;

		// Token: 0x02000671 RID: 1649
		public enum VoiceSounds
		{
			// Token: 0x0400232F RID: 9007
			normal,
			// Token: 0x04002330 RID: 9008
			surprise,
			// Token: 0x04002331 RID: 9009
			death
		}
	}
}
