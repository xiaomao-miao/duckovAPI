using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003D2 RID: 978
	public class MaterialPropertyFade : FadeElement
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x0007C1C0 File Offset: 0x0007A3C0
		// (set) Token: 0x06002381 RID: 9089 RVA: 0x0007C1C8 File Offset: 0x0007A3C8
		public AnimationCurve ShowCurve
		{
			get
			{
				return this.showCurve;
			}
			set
			{
				this.showCurve = value;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x0007C1D1 File Offset: 0x0007A3D1
		// (set) Token: 0x06002383 RID: 9091 RVA: 0x0007C1D9 File Offset: 0x0007A3D9
		public AnimationCurve HideCurve
		{
			get
			{
				return this.hideCurve;
			}
			set
			{
				this.hideCurve = value;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x0007C1E4 File Offset: 0x0007A3E4
		private Material Material
		{
			get
			{
				if (this._material == null && this.renderer != null)
				{
					this._material = UnityEngine.Object.Instantiate<Material>(this.renderer.material);
					this.renderer.material = this._material;
				}
				return this._material;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002385 RID: 9093 RVA: 0x0007C23A File Offset: 0x0007A43A
		// (set) Token: 0x06002386 RID: 9094 RVA: 0x0007C242 File Offset: 0x0007A442
		public float Duration
		{
			get
			{
				return this.duration;
			}
			internal set
			{
				this.duration = value;
			}
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0007C24B File Offset: 0x0007A44B
		private void Awake()
		{
			if (this.renderer == null)
			{
				this.renderer = base.GetComponent<Image>();
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0007C267 File Offset: 0x0007A467
		private void OnDestroy()
		{
			if (this._material)
			{
				UnityEngine.Object.Destroy(this._material);
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0007C284 File Offset: 0x0007A484
		protected override UniTask HideTask(int token)
		{
			MaterialPropertyFade.<HideTask>d__20 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.token = token;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<MaterialPropertyFade.<HideTask>d__20>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0007C2CF File Offset: 0x0007A4CF
		protected override void OnSkipHide()
		{
			if (this.Material == null)
			{
				return;
			}
			this.Material.SetFloat(this.propertyName, this.propertyRange.x);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0007C2FC File Offset: 0x0007A4FC
		protected override void OnSkipShow()
		{
			if (this.Material == null)
			{
				return;
			}
			this.Material.SetFloat(this.propertyName, this.propertyRange.y);
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0007C32C File Offset: 0x0007A52C
		protected override UniTask ShowTask(int token)
		{
			MaterialPropertyFade.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.token = token;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<MaterialPropertyFade.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0007C3F4 File Offset: 0x0007A5F4
		[CompilerGenerated]
		internal static float <HideTask>g__TimeSinceFadeBegun|20_0(ref MaterialPropertyFade.<>c__DisplayClass20_0 A_0)
		{
			return Time.unscaledTime - A_0.timeWhenFadeBegun;
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0007C402 File Offset: 0x0007A602
		[CompilerGenerated]
		internal static float <ShowTask>g__TimeSinceFadeBegun|23_0(ref MaterialPropertyFade.<>c__DisplayClass23_0 A_0)
		{
			return Time.unscaledTime - A_0.timeWhenFadeBegun;
		}

		// Token: 0x0400181A RID: 6170
		[SerializeField]
		private Image renderer;

		// Token: 0x0400181B RID: 6171
		[SerializeField]
		private string propertyName = "t";

		// Token: 0x0400181C RID: 6172
		[SerializeField]
		private Vector2 propertyRange = new Vector2(0f, 1f);

		// Token: 0x0400181D RID: 6173
		[SerializeField]
		private float duration = 0.5f;

		// Token: 0x0400181E RID: 6174
		[SerializeField]
		private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400181F RID: 6175
		[SerializeField]
		private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001820 RID: 6176
		private Material _material;
	}
}
