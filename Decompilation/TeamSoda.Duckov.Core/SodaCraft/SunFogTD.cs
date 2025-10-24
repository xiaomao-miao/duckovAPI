using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x0200041D RID: 1053
	[VolumeComponentMenu("SodaCraft/SunFogTD")]
	[Serializable]
	public class SunFogTD : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x060025D9 RID: 9689 RVA: 0x0008248D File Offset: 0x0008068D
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x0008249A File Offset: 0x0008069A
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000824A0 File Offset: 0x000806A0
		public override void Override(VolumeComponent state, float interpFactor)
		{
			SunFogTD sunFogTD = state as SunFogTD;
			base.Override(state, interpFactor);
			Shader.SetGlobalColor(this.fogColorHash, sunFogTD.fogColor.value);
			Shader.SetGlobalColor(this.sunColorHash, sunFogTD.sunColor.value);
			Shader.SetGlobalFloat(this.nearDistanceHash, sunFogTD.clipPlanes.value.x);
			Shader.SetGlobalFloat(this.farDistanceHash, sunFogTD.clipPlanes.value.y);
			Shader.SetGlobalFloat(this.sunSizeHash, sunFogTD.sunSize.value);
			Shader.SetGlobalFloat(this.sunPowerHash, sunFogTD.sunPower.value);
			Shader.SetGlobalVector(this.sunPointHash, sunFogTD.sunPoint.value);
			Shader.SetGlobalFloat(this.sunAlphaGainHash, sunFogTD.sunAlphaGain.value);
		}

		// Token: 0x040019B8 RID: 6584
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x040019B9 RID: 6585
		public ColorParameter fogColor = new ColorParameter(new Color(0.68718916f, 1.070217f, 1.3615336f, 0f), true, true, false, false);

		// Token: 0x040019BA RID: 6586
		public ColorParameter sunColor = new ColorParameter(new Color(4.061477f, 2.5092788f, 1.7816858f, 0f), true, true, false, false);

		// Token: 0x040019BB RID: 6587
		public FloatRangeParameter clipPlanes = new FloatRangeParameter(new Vector2(41f, 72f), 0.3f, 1000f, false);

		// Token: 0x040019BC RID: 6588
		public Vector2Parameter sunPoint = new Vector2Parameter(new Vector2(-2.63f, 1.23f), false);

		// Token: 0x040019BD RID: 6589
		public FloatParameter sunSize = new ClampedFloatParameter(1.85f, 0f, 10f, false);

		// Token: 0x040019BE RID: 6590
		public ClampedFloatParameter sunPower = new ClampedFloatParameter(1f, 0.1f, 10f, false);

		// Token: 0x040019BF RID: 6591
		public ClampedFloatParameter sunAlphaGain = new ClampedFloatParameter(0.001f, 0f, 0.25f, false);

		// Token: 0x040019C0 RID: 6592
		private int fogColorHash = Shader.PropertyToID("SunFogColor");

		// Token: 0x040019C1 RID: 6593
		private int sunColorHash = Shader.PropertyToID("SunFogSunColor");

		// Token: 0x040019C2 RID: 6594
		private int nearDistanceHash = Shader.PropertyToID("SunFogNearDistance");

		// Token: 0x040019C3 RID: 6595
		private int farDistanceHash = Shader.PropertyToID("SunFogFarDistance");

		// Token: 0x040019C4 RID: 6596
		private int sunPointHash = Shader.PropertyToID("SunFogSunPoint");

		// Token: 0x040019C5 RID: 6597
		private int sunSizeHash = Shader.PropertyToID("SunFogSunSize");

		// Token: 0x040019C6 RID: 6598
		private int sunPowerHash = Shader.PropertyToID("SunFogSunPower");

		// Token: 0x040019C7 RID: 6599
		private int sunAlphaGainHash = Shader.PropertyToID("SunFogSunAplhaGain");
	}
}
