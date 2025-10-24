using System;
using Duckov.UI;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000268 RID: 616
	public class NoteInteract : InteractableBase
	{
		// Token: 0x06001325 RID: 4901 RVA: 0x0004755F File Offset: 0x0004575F
		protected override void Start()
		{
			base.Start();
			if (NoteIndex.GetNoteUnlocked(this.noteKey))
			{
				base.gameObject.SetActive(false);
			}
			this.finishWhenTimeOut = true;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00047587 File Offset: 0x00045787
		protected override void OnInteractFinished()
		{
			NoteIndex.SetNoteUnlocked(this.noteKey);
			NoteIndexView.ShowNote(this.noteKey, true);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000475AC File Offset: 0x000457AC
		private void OnValidate()
		{
			this.noteTitle = "Note_" + this.noteKey + "_Title";
			this.noteContent = "Note_" + this.noteKey + "_Content";
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000475E4 File Offset: 0x000457E4
		public void ReName()
		{
			base.gameObject.name = "Note_" + this.noteKey;
		}

		// Token: 0x04000E49 RID: 3657
		public string noteKey;

		// Token: 0x04000E4A RID: 3658
		[LocalizationKey("Default")]
		public string noteTitle;

		// Token: 0x04000E4B RID: 3659
		[LocalizationKey("Default")]
		public string noteContent;
	}
}
