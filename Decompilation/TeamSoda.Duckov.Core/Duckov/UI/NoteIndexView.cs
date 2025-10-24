using System;
using Duckov.NoteIndexs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000387 RID: 903
	public class NoteIndexView : View
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x0006DD28 File Offset: 0x0006BF28
		private PrefabPool<NoteIndexView_Entry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<NoteIndexView_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0006DD61 File Offset: 0x0006BF61
		private void OnEnable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Combine(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0006DD83 File Offset: 0x0006BF83
		private void OnDisable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Remove(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0006DDA5 File Offset: 0x0006BFA5
		private void Update()
		{
			if (this.needFocus)
			{
				this.needFocus = false;
				this.MoveScrollViewToActiveEntry();
			}
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0006DDBC File Offset: 0x0006BFBC
		private void OnNoteStatusChanged(string noteKey)
		{
			this.RefreshEntries();
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0006DDC4 File Offset: 0x0006BFC4
		public void DoOpen()
		{
			base.Open(null);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0006DDCD File Offset: 0x0006BFCD
		protected override void OnOpen()
		{
			base.OnOpen();
			this.mainFadeGroup.Show();
			this.RefreshEntries();
			this.SetDisplayTargetNote(this.displayingNote);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0006DDF2 File Offset: 0x0006BFF2
		protected override void OnClose()
		{
			base.OnClose();
			this.mainFadeGroup.Hide();
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0006DE05 File Offset: 0x0006C005
		protected override void OnCancel()
		{
			base.Close();
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0006DE10 File Offset: 0x0006C010
		private void RefreshNoteCount()
		{
			int totalNoteCount = NoteIndex.GetTotalNoteCount();
			int unlockedNoteCount = NoteIndex.GetUnlockedNoteCount();
			this.noteCountText.text = string.Format("{0} / {1}", unlockedNoteCount, totalNoteCount);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0006DE4C File Offset: 0x0006C04C
		private void RefreshEntries()
		{
			this.RefreshNoteCount();
			this.Pool.ReleaseAll();
			if (NoteIndex.Instance == null)
			{
				return;
			}
			int num = 0;
			foreach (string key in NoteIndex.GetAllNotes(false))
			{
				Note note = NoteIndex.GetNote(key);
				if (note != null)
				{
					NoteIndexView_Entry noteIndexView_Entry = this.Pool.Get(null);
					num++;
					noteIndexView_Entry.Setup(note, new Action<NoteIndexView_Entry>(this.OnEntryClicked), new Func<string>(this.GetDisplayingNote), num);
				}
			}
			this.noEntryIndicator.SetActive(num <= 0);
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0006DEFC File Offset: 0x0006C0FC
		private string GetDisplayingNote()
		{
			return this.displayingNote;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0006DF04 File Offset: 0x0006C104
		public void SetDisplayTargetNote(string noteKey)
		{
			Note note = null;
			if (!string.IsNullOrWhiteSpace(noteKey))
			{
				note = NoteIndex.GetNote(noteKey);
			}
			if (note == null)
			{
				this.displayingNote = null;
			}
			else
			{
				this.displayingNote = note.key;
			}
			foreach (NoteIndexView_Entry noteIndexView_Entry in this.Pool.ActiveEntries)
			{
				noteIndexView_Entry.NotifySelectedDisplayingNoteChanged(this.displayingNote);
			}
			this.inspector.Setup(note);
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0006DF90 File Offset: 0x0006C190
		private void OnEntryClicked(NoteIndexView_Entry entry)
		{
			string key = entry.key;
			if (!NoteIndex.GetNoteUnlocked(key))
			{
				this.SetDisplayTargetNote("");
				return;
			}
			this.SetDisplayTargetNote(key);
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0006DFC0 File Offset: 0x0006C1C0
		public static void ShowNote(string noteKey, bool unlock = true)
		{
			NoteIndexView viewInstance = View.GetViewInstance<NoteIndexView>();
			if (viewInstance == null)
			{
				return;
			}
			if (unlock)
			{
				NoteIndex.SetNoteUnlocked(noteKey);
			}
			if (!(View.ActiveView is NoteIndexView))
			{
				viewInstance.Open(null);
			}
			viewInstance.SetDisplayTargetNote(noteKey);
			viewInstance.needFocus = true;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0006E008 File Offset: 0x0006C208
		private void MoveScrollViewToActiveEntry()
		{
			NoteIndexView_Entry displayingEntry = this.GetDisplayingEntry();
			if (displayingEntry == null)
			{
				return;
			}
			RectTransform rectTransform = displayingEntry.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			float num = -rectTransform.anchoredPosition.y;
			float height = this.indexScrollView.content.rect.height;
			float verticalNormalizedPosition = 1f - num / height;
			this.indexScrollView.verticalNormalizedPosition = verticalNormalizedPosition;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0006E07C File Offset: 0x0006C27C
		private NoteIndexView_Entry GetDisplayingEntry()
		{
			foreach (NoteIndexView_Entry noteIndexView_Entry in this.Pool.ActiveEntries)
			{
				if (noteIndexView_Entry.key == this.displayingNote)
				{
					return noteIndexView_Entry;
				}
			}
			return null;
		}

		// Token: 0x04001569 RID: 5481
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x0400156A RID: 5482
		[SerializeField]
		private GameObject noEntryIndicator;

		// Token: 0x0400156B RID: 5483
		[SerializeField]
		private NoteIndexView_Entry entryTemplate;

		// Token: 0x0400156C RID: 5484
		[SerializeField]
		private NoteIndexView_Inspector inspector;

		// Token: 0x0400156D RID: 5485
		[SerializeField]
		private TextMeshProUGUI noteCountText;

		// Token: 0x0400156E RID: 5486
		[SerializeField]
		private ScrollRect indexScrollView;

		// Token: 0x0400156F RID: 5487
		private PrefabPool<NoteIndexView_Entry> _pool;

		// Token: 0x04001570 RID: 5488
		private string displayingNote;

		// Token: 0x04001571 RID: 5489
		private bool needFocus;
	}
}
