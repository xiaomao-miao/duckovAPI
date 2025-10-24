using System;
using Saves;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033A RID: 826
	[Serializable]
	public abstract class Task : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x000669B3 File Offset: 0x00064BB3
		// (set) Token: 0x06001C6A RID: 7274 RVA: 0x000669BB File Offset: 0x00064BBB
		public Quest Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x000669C4 File Offset: 0x00064BC4
		// (set) Token: 0x06001C6C RID: 7276 RVA: 0x000669CC File Offset: 0x00064BCC
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x000669D5 File Offset: 0x00064BD5
		public virtual string Description
		{
			get
			{
				return "未定义Task描述。";
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x000669DC File Offset: 0x00064BDC
		public virtual string[] ExtraDescriptsions
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x000669E4 File Offset: 0x00064BE4
		public virtual Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06001C70 RID: 7280 RVA: 0x000669E8 File Offset: 0x00064BE8
		// (remove) Token: 0x06001C71 RID: 7281 RVA: 0x00066A20 File Offset: 0x00064C20
		public event Action<Task> onStatusChanged;

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x00066A55 File Offset: 0x00064C55
		public virtual bool Interactable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001C73 RID: 7283 RVA: 0x00066A58 File Offset: 0x00064C58
		public virtual bool PossibleValidInteraction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x00066A5B File Offset: 0x00064C5B
		public virtual bool NeedInspection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001C75 RID: 7285 RVA: 0x00066A5E File Offset: 0x00064C5E
		public virtual string InteractText
		{
			get
			{
				return "交互";
			}
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00066A65 File Offset: 0x00064C65
		public virtual void Interact()
		{
			Debug.LogWarning(string.Format("{0}可能未定义交互行为", base.GetType()));
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00066A7C File Offset: 0x00064C7C
		public bool IsFinished()
		{
			return this.forceFinish || this.CheckFinished();
		}

		// Token: 0x06001C78 RID: 7288
		protected abstract bool CheckFinished();

		// Token: 0x06001C79 RID: 7289
		public abstract object GenerateSaveData();

		// Token: 0x06001C7A RID: 7290
		public abstract void SetupSaveData(object data);

		// Token: 0x06001C7B RID: 7291 RVA: 0x00066A8E File Offset: 0x00064C8E
		protected void ReportStatusChanged()
		{
			Action<Task> action = this.onStatusChanged;
			if (action != null)
			{
				action(this);
			}
			if (this.IsFinished())
			{
				Quest quest = this.Master;
				if (quest == null)
				{
					return;
				}
				quest.NotifyTaskFinished(this);
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00066ABB File Offset: 0x00064CBB
		internal void Init()
		{
			if (this.IsFinished())
			{
				base.enabled = false;
				return;
			}
			this.OnInit();
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00066AD3 File Offset: 0x00064CD3
		protected virtual void OnInit()
		{
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00066AD5 File Offset: 0x00064CD5
		internal void ForceFinish()
		{
			this.forceFinish = true;
			Action<Task> action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040013D3 RID: 5075
		[SerializeField]
		private Quest master;

		// Token: 0x040013D4 RID: 5076
		[SerializeField]
		private int id;

		// Token: 0x040013D6 RID: 5078
		[SerializeField]
		private bool forceFinish;
	}
}
