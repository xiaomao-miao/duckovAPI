using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002E6 RID: 742
	public class CropAnimator : MonoBehaviour
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x00057402 File Offset: 0x00055602
		private ParticleSystem PlantFX
		{
			get
			{
				return this.plantFX;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0005740A File Offset: 0x0005560A
		private ParticleSystem StageChangeFX
		{
			get
			{
				return this.stageChangeFX;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x00057412 File Offset: 0x00055612
		private ParticleSystem RipenFX
		{
			get
			{
				return this.ripenFX;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0005741A File Offset: 0x0005561A
		private ParticleSystem WaterFX
		{
			get
			{
				return this.waterFX;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x00057422 File Offset: 0x00055622
		private ParticleSystem HarvestFX
		{
			get
			{
				return this.harvestFX;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x0005742A File Offset: 0x0005562A
		private ParticleSystem DestroyFX
		{
			get
			{
				return this.destroyFX;
			}
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00057434 File Offset: 0x00055634
		private void Awake()
		{
			if (this.crop == null)
			{
				this.crop = base.GetComponent<Crop>();
			}
			Crop crop = this.crop;
			crop.onPlant = (Action<Crop>)Delegate.Combine(crop.onPlant, new Action<Crop>(this.OnPlant));
			Crop crop2 = this.crop;
			crop2.onRipen = (Action<Crop>)Delegate.Combine(crop2.onRipen, new Action<Crop>(this.OnRipen));
			Crop crop3 = this.crop;
			crop3.onWater = (Action<Crop>)Delegate.Combine(crop3.onWater, new Action<Crop>(this.OnWater));
			Crop crop4 = this.crop;
			crop4.onHarvest = (Action<Crop>)Delegate.Combine(crop4.onHarvest, new Action<Crop>(this.OnHarvest));
			Crop crop5 = this.crop;
			crop5.onBeforeDestroy = (Action<Crop>)Delegate.Combine(crop5.onBeforeDestroy, new Action<Crop>(this.OnBeforeDestroy));
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0005751E File Offset: 0x0005571E
		private void Update()
		{
			this.RefreshPosition(true);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00057528 File Offset: 0x00055728
		private void RefreshPosition(bool notifyStageChange = true)
		{
			float progress = this.crop.Progress;
			CropAnimator.Stage stage = default(CropAnimator.Stage);
			int? num = this.cachedStage;
			for (int i = 0; i < this.stages.Length; i++)
			{
				CropAnimator.Stage stage2 = this.stages[i];
				if (progress < this.stages[i].progress)
				{
					stage = stage2;
					this.cachedStage = new int?(i);
					break;
				}
			}
			this.displayParent.localPosition = Vector3.up * stage.position;
			if (!notifyStageChange)
			{
				return;
			}
			if (num == null)
			{
				return;
			}
			int value = num.Value;
			int? num2 = this.cachedStage;
			if (!(value == num2.GetValueOrDefault() & num2 != null))
			{
				this.OnStageChange();
			}
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x000575E7 File Offset: 0x000557E7
		private void OnStageChange()
		{
			FXPool.Play(this.StageChangeFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0005760B File Offset: 0x0005580B
		private void OnWater(Crop crop)
		{
			FXPool.Play(this.WaterFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0005762F File Offset: 0x0005582F
		private void OnRipen(Crop crop)
		{
			FXPool.Play(this.RipenFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00057653 File Offset: 0x00055853
		private void OnHarvest(Crop crop)
		{
			FXPool.Play(this.HarvestFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00057677 File Offset: 0x00055877
		private void OnPlant(Crop crop)
		{
			FXPool.Play(this.PlantFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0005769B File Offset: 0x0005589B
		private void OnBeforeDestroy(Crop crop)
		{
			FXPool.Play(this.DestroyFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x0400115C RID: 4444
		[SerializeField]
		private Crop crop;

		// Token: 0x0400115D RID: 4445
		[SerializeField]
		private Transform displayParent;

		// Token: 0x0400115E RID: 4446
		[SerializeField]
		private ParticleSystem plantFX;

		// Token: 0x0400115F RID: 4447
		[SerializeField]
		private ParticleSystem stageChangeFX;

		// Token: 0x04001160 RID: 4448
		[SerializeField]
		private ParticleSystem ripenFX;

		// Token: 0x04001161 RID: 4449
		[SerializeField]
		private ParticleSystem waterFX;

		// Token: 0x04001162 RID: 4450
		[SerializeField]
		private ParticleSystem harvestFX;

		// Token: 0x04001163 RID: 4451
		[SerializeField]
		private ParticleSystem destroyFX;

		// Token: 0x04001164 RID: 4452
		[SerializeField]
		private CropAnimator.Stage[] stages = new CropAnimator.Stage[]
		{
			new CropAnimator.Stage(0.333f, -0.4f),
			new CropAnimator.Stage(0.666f, -0.2f),
			new CropAnimator.Stage(0.999f, -0.1f)
		};

		// Token: 0x04001165 RID: 4453
		private int? cachedStage;

		// Token: 0x02000580 RID: 1408
		[Serializable]
		private struct Stage
		{
			// Token: 0x06002860 RID: 10336 RVA: 0x00095056 File Offset: 0x00093256
			public Stage(float progress, float position)
			{
				this.progress = progress;
				this.position = position;
			}

			// Token: 0x04001FB0 RID: 8112
			public float progress;

			// Token: 0x04001FB1 RID: 8113
			public float position;
		}
	}
}
