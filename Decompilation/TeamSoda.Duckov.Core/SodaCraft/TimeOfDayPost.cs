using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x02000421 RID: 1057
	[VolumeComponentMenu("SodaCraft/TimeOfDayPost")]
	[Serializable]
	public class TimeOfDayPost : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x060025E8 RID: 9704 RVA: 0x00082BC5 File Offset: 0x00080DC5
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00082BD2 File Offset: 0x00080DD2
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x00082BD8 File Offset: 0x00080DD8
		public override void Override(VolumeComponent state, float interpFactor)
		{
			TimeOfDayPost timeOfDayPost = state as TimeOfDayPost;
			base.Override(state, interpFactor);
			if (timeOfDayPost == null)
			{
				return;
			}
			TimeOfDayController.NightViewAngleFactor = timeOfDayPost.nightViewAngleFactor.value;
			TimeOfDayController.NightViewDistanceFactor = timeOfDayPost.nightViewDistanceFactor.value;
			TimeOfDayController.NightSenseRangeFactor = timeOfDayPost.nightSenseRangeFactor.value;
		}

		// Token: 0x040019E7 RID: 6631
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x040019E8 RID: 6632
		public ClampedFloatParameter nightViewAngleFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x040019E9 RID: 6633
		public ClampedFloatParameter nightViewDistanceFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x040019EA RID: 6634
		public ClampedFloatParameter nightSenseRangeFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);
	}
}
