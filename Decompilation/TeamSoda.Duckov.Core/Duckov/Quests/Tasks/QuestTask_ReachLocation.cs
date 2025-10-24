using System;
using Duckov.Scenes;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000353 RID: 851
	public class QuestTask_ReachLocation : Task
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0006985E File Offset: 0x00067A5E
		public string descriptionFormatkey
		{
			get
			{
				return "Task_ReachLocation";
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x00069865 File Offset: 0x00067A65
		public string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatkey.ToPlainText();
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x00069872 File Offset: 0x00067A72
		public string TargetLocationDisplayName
		{
			get
			{
				return this.location.GetDisplayName();
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x0006987F File Offset: 0x00067A7F
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.TargetLocationDisplayName
				});
			}
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x00069897 File Offset: 0x00067A97
		private void OnEnable()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000698BB File Offset: 0x00067ABB
		private void Start()
		{
			this.CacheLocation();
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000698C3 File Offset: 0x00067AC3
		private void OnDisable()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x000698E7 File Offset: 0x00067AE7
		protected override void OnInit()
		{
			base.OnInit();
			if (!base.IsFinished())
			{
				this.SetMapElementVisable(true);
			}
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x000698FE File Offset: 0x00067AFE
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.CacheLocation();
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x00069906 File Offset: 0x00067B06
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Reach location task caching";
			this.CacheLocation();
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00069918 File Offset: 0x00067B18
		private void CacheLocation()
		{
			this.target = this.location.GetLocationTransform();
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0006992C File Offset: 0x00067B2C
		private void Update()
		{
			if (this.finished)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			if ((main.transform.position - this.target.position).magnitude <= this.radius)
			{
				this.finished = true;
				this.SetMapElementVisable(false);
			}
			base.ReportStatusChanged();
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000699A0 File Offset: 0x00067BA0
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000699AD File Offset: 0x00067BAD
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000699B8 File Offset: 0x00067BB8
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000699DC File Offset: 0x00067BDC
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.locations.Clear();
				this.mapElement.locations.Add(this.location);
				this.mapElement.range = this.radius;
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x04001478 RID: 5240
		[SerializeField]
		private MultiSceneLocation location;

		// Token: 0x04001479 RID: 5241
		[SerializeField]
		private float radius = 1f;

		// Token: 0x0400147A RID: 5242
		[SerializeField]
		private bool finished;

		// Token: 0x0400147B RID: 5243
		[SerializeField]
		private Transform target;

		// Token: 0x0400147C RID: 5244
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
