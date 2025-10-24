using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000264 RID: 612
	public class NoteIndex : MonoBehaviour
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x000471F8 File Offset: 0x000453F8
		public static NoteIndex Instance
		{
			get
			{
				return GameManager.NoteIndex;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001305 RID: 4869 RVA: 0x000471FF File Offset: 0x000453FF
		public List<Note> Notes
		{
			get
			{
				return this.notes;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x00047207 File Offset: 0x00045407
		private Dictionary<string, Note> MDic
		{
			get
			{
				if (this._dic == null)
				{
					this.RebuildDic();
				}
				return this._dic;
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00047220 File Offset: 0x00045420
		private void RebuildDic()
		{
			if (this._dic == null)
			{
				this._dic = new Dictionary<string, Note>();
			}
			this._dic.Clear();
			foreach (Note note in this.notes)
			{
				this._dic[note.key] = note;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x0004729C File Offset: 0x0004549C
		public HashSet<string> UnlockedNotes
		{
			get
			{
				return this.unlockedNotes;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x000472A4 File Offset: 0x000454A4
		public HashSet<string> ReadNotes
		{
			get
			{
				return this.unlockedNotes;
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000472AC File Offset: 0x000454AC
		public static IEnumerable<string> GetAllNotes(bool unlockedOnly = true)
		{
			if (NoteIndex.Instance == null)
			{
				yield break;
			}
			foreach (Note note in NoteIndex.Instance.notes)
			{
				string key = note.key;
				if (!unlockedOnly || NoteIndex.GetNoteUnlocked(key))
				{
					yield return note.key;
				}
			}
			List<Note>.Enumerator enumerator = default(List<Note>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000472BC File Offset: 0x000454BC
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
			this.Load();
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000472E6 File Offset: 0x000454E6
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0004730C File Offset: 0x0004550C
		private void Save()
		{
			NoteIndex.SaveData value = new NoteIndex.SaveData(this);
			SavesSystem.Save<NoteIndex.SaveData>("NoteIndexData", value);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0004732C File Offset: 0x0004552C
		private void Load()
		{
			SavesSystem.Load<NoteIndex.SaveData>("NoteIndexData").Setup(this);
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0004734C File Offset: 0x0004554C
		public void MSetEntryDynamic(Note note)
		{
			this.MDic[note.key] = note;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00047360 File Offset: 0x00045560
		public Note MGetNote(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogError("Trying to get note with an empty key.");
				return null;
			}
			Note result;
			if (!this.MDic.TryGetValue(key, out result))
			{
				Debug.LogError("Cannot find note: " + key);
				return null;
			}
			return result;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000473A4 File Offset: 0x000455A4
		public static Note GetNote(string key)
		{
			if (NoteIndex.Instance == null)
			{
				return null;
			}
			return NoteIndex.Instance.MGetNote(key);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000473C0 File Offset: 0x000455C0
		public static bool SetNoteDynamic(Note note)
		{
			if (NoteIndex.Instance == null)
			{
				return false;
			}
			NoteIndex.Instance.MSetEntryDynamic(note);
			return true;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x000473DD File Offset: 0x000455DD
		public static bool GetNoteUnlocked(string noteKey)
		{
			return !(NoteIndex.Instance == null) && NoteIndex.Instance.unlockedNotes.Contains(noteKey);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x000473FE File Offset: 0x000455FE
		public static bool GetNoteRead(string noteKey)
		{
			return !(NoteIndex.Instance == null) && NoteIndex.Instance.readNotes.Contains(noteKey);
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0004741F File Offset: 0x0004561F
		public static void SetNoteUnlocked(string noteKey)
		{
			if (NoteIndex.Instance == null)
			{
				return;
			}
			NoteIndex.Instance.unlockedNotes.Add(noteKey);
			Action<string> action = NoteIndex.onNoteStatusChanged;
			if (action == null)
			{
				return;
			}
			action(noteKey);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00047450 File Offset: 0x00045650
		public static void SetNoteRead(string noteKey)
		{
			if (NoteIndex.Instance == null)
			{
				return;
			}
			NoteIndex.Instance.readNotes.Add(noteKey);
			Action<string> action = NoteIndex.onNoteStatusChanged;
			if (action == null)
			{
				return;
			}
			action(noteKey);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00047481 File Offset: 0x00045681
		internal static int GetTotalNoteCount()
		{
			if (NoteIndex.Instance == null)
			{
				return 0;
			}
			return NoteIndex.Instance.Notes.Count<Note>();
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x000474A1 File Offset: 0x000456A1
		internal static int GetUnlockedNoteCount()
		{
			if (NoteIndex.Instance == null)
			{
				return 0;
			}
			return NoteIndex.Instance.UnlockedNotes.Count;
		}

		// Token: 0x04000E3E RID: 3646
		[SerializeField]
		private List<Note> notes = new List<Note>();

		// Token: 0x04000E3F RID: 3647
		private Dictionary<string, Note> _dic;

		// Token: 0x04000E40 RID: 3648
		private HashSet<string> unlockedNotes = new HashSet<string>();

		// Token: 0x04000E41 RID: 3649
		private HashSet<string> readNotes = new HashSet<string>();

		// Token: 0x04000E42 RID: 3650
		public static Action<string> onNoteStatusChanged;

		// Token: 0x04000E43 RID: 3651
		private const string SaveKey = "NoteIndexData";

		// Token: 0x0200053A RID: 1338
		[Serializable]
		private struct SaveData
		{
			// Token: 0x060027CF RID: 10191 RVA: 0x00091ACF File Offset: 0x0008FCCF
			public SaveData(NoteIndex from)
			{
				this.unlockedNotes = from.unlockedNotes.ToList<string>();
				this.readNotes = from.unlockedNotes.ToList<string>();
			}

			// Token: 0x060027D0 RID: 10192 RVA: 0x00091AF4 File Offset: 0x0008FCF4
			public void Setup(NoteIndex to)
			{
				to.unlockedNotes.Clear();
				if (this.unlockedNotes != null)
				{
					to.unlockedNotes.AddRange(this.unlockedNotes);
				}
				to.readNotes.Clear();
				if (this.readNotes != null)
				{
					to.readNotes.AddRange(this.readNotes);
				}
			}

			// Token: 0x04001E8A RID: 7818
			public List<string> unlockedNotes;

			// Token: 0x04001E8B RID: 7819
			public List<string> readNotes;
		}
	}
}
