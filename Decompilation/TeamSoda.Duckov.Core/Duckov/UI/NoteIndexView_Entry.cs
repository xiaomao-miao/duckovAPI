using System;
using Duckov.NoteIndexs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000388 RID: 904
	public class NoteIndexView_Entry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0006E0EC File Offset: 0x0006C2EC
		public string key
		{
			get
			{
				return this.note.key;
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0006E0F9 File Offset: 0x0006C2F9
		private void OnEnable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Combine(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0006E11B File Offset: 0x0006C31B
		private void OnDisable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Remove(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0006E13D File Offset: 0x0006C33D
		private void OnNoteStatusChanged(string key)
		{
			if (key != this.note.key)
			{
				return;
			}
			this.RefreshNotReadIndicator();
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0006E159 File Offset: 0x0006C359
		private void RefreshNotReadIndicator()
		{
			this.notReadIndicator.SetActive(NoteIndex.GetNoteUnlocked(this.key) && !NoteIndex.GetNoteRead(this.key));
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0006E184 File Offset: 0x0006C384
		internal void NotifySelectedDisplayingNoteChanged(string displayingNote)
		{
			this.RefreshHighlight();
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0006E18C File Offset: 0x0006C38C
		private void RefreshHighlight()
		{
			bool active = false;
			if (this.getDisplayingNote != null)
			{
				Func<string> func = this.getDisplayingNote;
				active = (((func != null) ? func() : null) == this.key);
			}
			this.highlightIndicator.SetActive(active);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0006E1D0 File Offset: 0x0006C3D0
		internal void Setup(Note note, Action<NoteIndexView_Entry> onClicked, Func<string> getDisplayingNote, int index)
		{
			bool noteUnlocked = NoteIndex.GetNoteUnlocked(note.key);
			this.note = note;
			this.titleText.text = (noteUnlocked ? note.Title : "???");
			this.onClicked = onClicked;
			this.getDisplayingNote = getDisplayingNote;
			if (index > 0)
			{
				this.indexText.text = index.ToString("000");
			}
			this.RefreshNotReadIndicator();
			this.RefreshHighlight();
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0006E241 File Offset: 0x0006C441
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<NoteIndexView_Entry> action = this.onClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x04001572 RID: 5490
		[SerializeField]
		private GameObject highlightIndicator;

		// Token: 0x04001573 RID: 5491
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x04001574 RID: 5492
		[SerializeField]
		private TextMeshProUGUI indexText;

		// Token: 0x04001575 RID: 5493
		[SerializeField]
		private GameObject notReadIndicator;

		// Token: 0x04001576 RID: 5494
		private Note note;

		// Token: 0x04001577 RID: 5495
		private Action<NoteIndexView_Entry> onClicked;

		// Token: 0x04001578 RID: 5496
		private Func<string> getDisplayingNote;
	}
}
