using System;
using Duckov.Scenes;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000351 RID: 849
	public class QuestTask_Evacuate : Task
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x00069276 File Offset: 0x00067476
		private SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x00069284 File Offset: 0x00067484
		private SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x000692A3 File Offset: 0x000674A3
		private string descriptionFormatKey
		{
			get
			{
				return "Task_Evacuate";
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x000692AA File Offset: 0x000674AA
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000692B8 File Offset: 0x000674B8
		private string TargetDisplayName
		{
			get
			{
				if (this.RequireScene != null && this.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.None)
				{
					return this.RequireSceneInfo.DisplayName;
				}
				if (base.Master.RequireScene != null && base.Master.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.None)
				{
					return base.Master.RequireSceneInfo.DisplayName;
				}
				return "Scene_Any".ToPlainText();
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x00069320 File Offset: 0x00067520
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.TargetDisplayName
				});
			}
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x00069338 File Offset: 0x00067538
		private void OnEnable()
		{
			LevelManager.OnEvacuated += this.OnEvacuated;
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x0006934B File Offset: 0x0006754B
		private void OnDisable()
		{
			LevelManager.OnEvacuated -= this.OnEvacuated;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x00069360 File Offset: 0x00067560
		private void OnEvacuated(EvacuationInfo info)
		{
			if (this.finished)
			{
				return;
			}
			if (this.RequireScene == null || this.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.Empty)
			{
				if (base.Master.SceneRequirementSatisfied)
				{
					this.finished = true;
					base.ReportStatusChanged();
					return;
				}
			}
			else if (this.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.None && this.RequireScene.LoadedScene.isLoaded)
			{
				this.finished = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000693D6 File Offset: 0x000675D6
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000693E3 File Offset: 0x000675E3
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000693EC File Offset: 0x000675EC
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x0400146B RID: 5227
		[SerializeField]
		[SceneID]
		private string requireSceneID;

		// Token: 0x0400146C RID: 5228
		[SerializeField]
		private bool finished;
	}
}
