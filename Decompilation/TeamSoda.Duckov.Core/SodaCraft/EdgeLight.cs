using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x0200041F RID: 1055
	[VolumeComponentMenu("SodaCraft/EdgeLight")]
	[Serializable]
	public class EdgeLight : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x060025E0 RID: 9696 RVA: 0x000827A9 File Offset: 0x000809A9
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000827B6 File Offset: 0x000809B6
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000827BC File Offset: 0x000809BC
		public override void Override(VolumeComponent state, float interpFactor)
		{
			EdgeLight edgeLight = state as EdgeLight;
			base.Override(state, interpFactor);
			Shader.SetGlobalVector(this.edgeLightDirectionHash, edgeLight.direction.value);
			Shader.SetGlobalFloat(this.widthHash, edgeLight.edgeLightWidth.value);
			Shader.SetGlobalFloat(this.fixHash, edgeLight.edgeLightFix.value);
			Shader.SetGlobalFloat(this.clampDistanceHash, edgeLight.EdgeLightClampDistance.value);
			Shader.SetGlobalColor(this.colorHash, edgeLight.edgeLightColor.value);
			Shader.SetGlobalFloat(this.edgeLightBlendScreenColorHash, edgeLight.blendScreenColor.value);
		}

		// Token: 0x040019CC RID: 6604
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x040019CD RID: 6605
		public Vector2Parameter direction = new Vector2Parameter(new Vector2(-1f, 1f), false);

		// Token: 0x040019CE RID: 6606
		public ClampedFloatParameter edgeLightWidth = new ClampedFloatParameter(0.001f, 0f, 0.05f, false);

		// Token: 0x040019CF RID: 6607
		public ClampedFloatParameter edgeLightFix = new ClampedFloatParameter(0.001f, 0f, 0.05f, false);

		// Token: 0x040019D0 RID: 6608
		public FloatParameter EdgeLightClampDistance = new ClampedFloatParameter(0.001f, 0.001f, 1f, false);

		// Token: 0x040019D1 RID: 6609
		public ColorParameter edgeLightColor = new ColorParameter(Color.white, true, false, false, false);

		// Token: 0x040019D2 RID: 6610
		public FloatParameter blendScreenColor = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x040019D3 RID: 6611
		private int edgeLightDirectionHash = Shader.PropertyToID("_EdgeLightDirection");

		// Token: 0x040019D4 RID: 6612
		private int widthHash = Shader.PropertyToID("_EdgeLightWidth");

		// Token: 0x040019D5 RID: 6613
		private int colorHash = Shader.PropertyToID("_EdgeLightColor");

		// Token: 0x040019D6 RID: 6614
		private int fixHash = Shader.PropertyToID("_EdgeLightFix");

		// Token: 0x040019D7 RID: 6615
		private int clampDistanceHash = Shader.PropertyToID("_EdgeLightClampDistance");

		// Token: 0x040019D8 RID: 6616
		private int edgeLightBlendScreenColorHash = Shader.PropertyToID("_EdgeLightBlendScreenColor");
	}
}
