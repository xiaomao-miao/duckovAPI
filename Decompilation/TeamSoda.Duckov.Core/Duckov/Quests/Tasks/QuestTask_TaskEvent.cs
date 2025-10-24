using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000355 RID: 853
	public class QuestTask_TaskEvent : Task
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x00069B41 File Offset: 0x00067D41
		public string EventKey
		{
			get
			{
				return this.eventKey;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x00069B49 File Offset: 0x00067D49
		public override string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00069B56 File Offset: 0x00067D56
		private void OnTaskEvent(string _key)
		{
			if (_key == this.eventKey)
			{
				this.finished = true;
				this.SetMapElementVisable(false);
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00069B7A File Offset: 0x00067D7A
		protected override void OnInit()
		{
			base.OnInit();
			TaskEvent.OnTaskEvent += this.OnTaskEvent;
			this.SetMapElementVisable(!base.IsFinished());
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00069BA2 File Offset: 0x00067DA2
		private void OnDisable()
		{
			TaskEvent.OnTaskEvent -= this.OnTaskEvent;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00069BB5 File Offset: 0x00067DB5
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00069BBD File Offset: 0x00067DBD
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00069BCC File Offset: 0x00067DCC
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00069BF0 File Offset: 0x00067DF0
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (!this.mapElement.enabled)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x04001481 RID: 5249
		[SerializeField]
		private string eventKey;

		// Token: 0x04001482 RID: 5250
		[SerializeField]
		[LocalizationKey("Quests")]
		private string description;

		// Token: 0x04001483 RID: 5251
		private bool finished;

		// Token: 0x04001484 RID: 5252
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
