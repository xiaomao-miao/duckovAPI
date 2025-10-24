using System;
using NodeCanvas.DialogueTrees;
using SodaCraft.Localizations;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x0200021D RID: 541
	[Serializable]
	public class LocalizedStatement : IStatement
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0003F27F File Offset: 0x0003D47F
		public string text
		{
			get
			{
				return this.textKey.ToPlainText();
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0003F28C File Offset: 0x0003D48C
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x0003F294 File Offset: 0x0003D494
		public string textKey
		{
			get
			{
				return this._textKey;
			}
			set
			{
				this._textKey = value;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0003F29D File Offset: 0x0003D49D
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x0003F2A5 File Offset: 0x0003D4A5
		public AudioClip audio
		{
			get
			{
				return this._audio;
			}
			set
			{
				this._audio = value;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0003F2AE File Offset: 0x0003D4AE
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x0003F2B6 File Offset: 0x0003D4B6
		public string meta
		{
			get
			{
				return this._meta;
			}
			set
			{
				this._meta = value;
			}
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003F2BF File Offset: 0x0003D4BF
		public LocalizedStatement()
		{
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0003F2DD File Offset: 0x0003D4DD
		public LocalizedStatement(string textKey)
		{
			this._textKey = textKey;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0003F302 File Offset: 0x0003D502
		public LocalizedStatement(string textKey, AudioClip audio)
		{
			this._textKey = textKey;
			this.audio = audio;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0003F32E File Offset: 0x0003D52E
		public LocalizedStatement(string textKey, AudioClip audio, string meta)
		{
			this._textKey = textKey;
			this.audio = audio;
			this.meta = meta;
		}

		// Token: 0x04000CF9 RID: 3321
		[SerializeField]
		private string _textKey = string.Empty;

		// Token: 0x04000CFA RID: 3322
		[SerializeField]
		private AudioClip _audio;

		// Token: 0x04000CFB RID: 3323
		[SerializeField]
		private string _meta = string.Empty;
	}
}
