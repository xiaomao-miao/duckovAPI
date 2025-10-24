using System;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000245 RID: 581
	[Serializable]
	public class Precipitation
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x00044DD6 File Offset: 0x00042FD6
		public float CloudyThreshold
		{
			get
			{
				return this.cloudyThreshold;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x00044DDE File Offset: 0x00042FDE
		public float RainyThreshold
		{
			get
			{
				return this.rainyThreshold;
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00044DE6 File Offset: 0x00042FE6
		public bool IsRainy(TimeSpan dayAndTime)
		{
			return this.Get(dayAndTime) > this.rainyThreshold;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00044DF7 File Offset: 0x00042FF7
		public bool IsCloudy(TimeSpan dayAndTime)
		{
			return this.Get(dayAndTime) > this.cloudyThreshold;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00044E08 File Offset: 0x00043008
		public float Get(TimeSpan dayAndTime)
		{
			Vector2 perlinNoiseCoord = this.GetPerlinNoiseCoord(dayAndTime);
			return Mathf.Clamp01(((Mathf.PerlinNoise(perlinNoiseCoord.x, perlinNoiseCoord.y) + Mathf.PerlinNoise(perlinNoiseCoord.x + 0.5f + 123.4f, perlinNoiseCoord.y - 567.8f)) / 2f - 0.5f) * this.contrast + 0.5f + this.offset);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00044E78 File Offset: 0x00043078
		public Vector2 GetPerlinNoiseCoord(TimeSpan dayAndTime)
		{
			float num = (float)(dayAndTime.Days % 3650) * 24f + (float)dayAndTime.Hours + (float)dayAndTime.Minutes / 60f;
			int num2 = dayAndTime.Days / 3650;
			return new Vector2(num * this.frequency, (float)(this.seed + num2));
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00044ED4 File Offset: 0x000430D4
		internal void SetSeed(int seed)
		{
			this.seed = seed;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00044EE0 File Offset: 0x000430E0
		public float Get()
		{
			TimeSpan now = GameClock.Now;
			return this.Get(now);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00044EFC File Offset: 0x000430FC
		public bool IsRainy()
		{
			TimeSpan now = GameClock.Now;
			return this.IsRainy(now);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00044F18 File Offset: 0x00043118
		public bool IsCloudy()
		{
			TimeSpan now = GameClock.Now;
			return this.IsCloudy(now);
		}

		// Token: 0x04000DE2 RID: 3554
		[SerializeField]
		private int seed;

		// Token: 0x04000DE3 RID: 3555
		[SerializeField]
		[Range(0f, 1f)]
		private float cloudyThreshold;

		// Token: 0x04000DE4 RID: 3556
		[SerializeField]
		[Range(0f, 1f)]
		private float rainyThreshold;

		// Token: 0x04000DE5 RID: 3557
		[SerializeField]
		private float frequency = 1f;

		// Token: 0x04000DE6 RID: 3558
		[SerializeField]
		private float offset;

		// Token: 0x04000DE7 RID: 3559
		[SerializeField]
		private float contrast = 1f;
	}
}
