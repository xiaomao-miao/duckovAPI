using System;
using UnityEngine.Rendering;

namespace SodaCraft
{
	// Token: 0x0200041E RID: 1054
	[VolumeComponentMenu("SodaCraft/CameraArmControl")]
	[Serializable]
	public class CameraArmControl : VolumeComponent
	{
		// Token: 0x060025DD RID: 9693 RVA: 0x000826FD File Offset: 0x000808FD
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x0008270A File Offset: 0x0008090A
		public override void Override(VolumeComponent state, float interpFactor)
		{
			CameraArmControl cameraArmControl = state as CameraArmControl;
			base.Override(state, interpFactor);
			CameraArm.globalPitch = cameraArmControl.pitch.value;
			CameraArm.globalYaw = cameraArmControl.yaw.value;
			CameraArm.globalDistance = cameraArmControl.distance.value;
		}

		// Token: 0x040019C8 RID: 6600
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x040019C9 RID: 6601
		public MinFloatParameter pitch = new MinFloatParameter(55f, 0f, false);

		// Token: 0x040019CA RID: 6602
		public FloatParameter yaw = new FloatParameter(-30f, false);

		// Token: 0x040019CB RID: 6603
		public MinFloatParameter distance = new MinFloatParameter(45f, 2f, false);
	}
}
