using System;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000244 RID: 580
	[Serializable]
	public class Storm
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00044C69 File Offset: 0x00042E69
		[TimeSpan]
		private long Period
		{
			get
			{
				return this.sleepTime + this.stage1Time + this.stage2Time;
			}
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00044C80 File Offset: 0x00042E80
		public int GetStormLevel(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			if (num < this.sleepTime)
			{
				return 0;
			}
			if (num < this.sleepTime + this.stage1Time)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00044CC4 File Offset: 0x00042EC4
		public TimeSpan GetStormETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			if (num < this.sleepTime)
			{
				return TimeSpan.FromTicks(this.sleepTime - num);
			}
			return TimeSpan.Zero;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00044D04 File Offset: 0x00042F04
		public TimeSpan GetStormIOverETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			return TimeSpan.FromTicks(this.sleepTime + this.stage1Time - num);
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00044D3C File Offset: 0x00042F3C
		public TimeSpan GetStormIIOverETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			return TimeSpan.FromTicks(this.Period - num);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00044D6C File Offset: 0x00042F6C
		public float GetSleepPercent(TimeSpan dayAndTime)
		{
			return (float)((dayAndTime.Ticks + this.offset) % this.Period) / (float)this.sleepTime;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00044D8C File Offset: 0x00042F8C
		public float GetStormRemainPercent(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period - this.sleepTime;
			return 1f - (float)num / ((float)this.stage1Time + (float)this.stage2Time);
		}

		// Token: 0x04000DDE RID: 3550
		[SerializeField]
		[TimeSpan]
		private long offset;

		// Token: 0x04000DDF RID: 3551
		[SerializeField]
		[TimeSpan]
		private long sleepTime;

		// Token: 0x04000DE0 RID: 3552
		[SerializeField]
		[TimeSpan]
		private long stage1Time;

		// Token: 0x04000DE1 RID: 3553
		[SerializeField]
		[TimeSpan]
		private long stage2Time;
	}
}
