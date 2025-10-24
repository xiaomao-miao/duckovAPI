using System;
using Umbra;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x02000420 RID: 1056
	[VolumeComponentMenu("SodaCraft/LightControl")]
	[Serializable]
	public class LightControl : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x060025E4 RID: 9700 RVA: 0x0008297F File Offset: 0x00080B7F
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x0008298C File Offset: 0x00080B8C
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00082990 File Offset: 0x00080B90
		public override void Override(VolumeComponent state, float interpFactor)
		{
			LightControl lightControl = state as LightControl;
			base.Override(state, interpFactor);
			RenderSettings.ambientSkyColor = lightControl.skyColor.value;
			RenderSettings.ambientEquatorColor = lightControl.equatorColor.value;
			RenderSettings.ambientGroundColor = lightControl.groundColor.value;
			Shader.SetGlobalColor(this.fowColorID, lightControl.fowColor.value);
			Shader.SetGlobalColor(this.SodaPointLight_EnviromentTintID, lightControl.SodaLightTint.value);
			if (!LightControl.light)
			{
				LightControl.light = RenderSettings.sun;
			}
			if (LightControl.light)
			{
				LightControl.light.color = lightControl.sunColor.value;
				LightControl.light.intensity = lightControl.sunIntensity.value;
				LightControl.light.transform.rotation = Quaternion.Euler(lightControl.sunRotation.value);
				if (!LightControl.lightShadows)
				{
					LightControl.lightShadows = LightControl.light.GetComponent<UmbraSoftShadows>();
				}
				if (LightControl.lightShadows)
				{
					float value = lightControl.sunShadowHardness.value;
					LightControl.lightShadows.profile.contactStrength = value;
				}
			}
		}

		// Token: 0x040019D9 RID: 6617
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x040019DA RID: 6618
		public ColorParameter skyColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x040019DB RID: 6619
		public ColorParameter equatorColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x040019DC RID: 6620
		public ColorParameter groundColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x040019DD RID: 6621
		public ColorParameter sunColor = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x040019DE RID: 6622
		public ColorParameter fowColor = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x040019DF RID: 6623
		public MinFloatParameter sunIntensity = new MinFloatParameter(1f, 0f, false);

		// Token: 0x040019E0 RID: 6624
		public ClampedFloatParameter sunShadowHardness = new ClampedFloatParameter(0.96f, 0f, 1f, false);

		// Token: 0x040019E1 RID: 6625
		public Vector3Parameter sunRotation = new Vector3Parameter(new Vector3(59f, 168f, 0f), false);

		// Token: 0x040019E2 RID: 6626
		public ColorParameter SodaLightTint = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x040019E3 RID: 6627
		private int SodaPointLight_EnviromentTintID = Shader.PropertyToID("SodaPointLight_EnviromentTint");

		// Token: 0x040019E4 RID: 6628
		private int fowColorID = Shader.PropertyToID("_SodaUnknowColor");

		// Token: 0x040019E5 RID: 6629
		private static Light light;

		// Token: 0x040019E6 RID: 6630
		private static UmbraSoftShadows lightShadows;
	}
}
