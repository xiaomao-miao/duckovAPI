using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C1 RID: 961
	public class WeightBarComplex : MonoBehaviour
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0007A6D6 File Offset: 0x000788D6
		private CharacterMainControl Target
		{
			get
			{
				if (!this.target)
				{
					LevelManager instance = LevelManager.Instance;
					this.target = ((instance != null) ? instance.MainCharacter : null);
				}
				return this.target;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x0007A702 File Offset: 0x00078902
		private float LightPercentage
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x0007A709 File Offset: 0x00078909
		private float SuperHeavyPercentage
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x0007A710 File Offset: 0x00078910
		private float MaxWeight
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.MaxWeight;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x0007A734 File Offset: 0x00078934
		private float BarWidth
		{
			get
			{
				if (this.barArea == null)
				{
					return 0f;
				}
				return this.barArea.rect.width;
			}
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0007A768 File Offset: 0x00078968
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			if (this.Target)
			{
				this.Target.CharacterItem.onChildChanged += this.OnTargetChildChanged;
			}
			this.RefreshMarkPositions();
			this.ResetMainBar();
			this.Animate().Forget();
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0007A7C6 File Offset: 0x000789C6
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			if (this.Target)
			{
				this.Target.CharacterItem.onChildChanged -= this.OnTargetChildChanged;
			}
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0007A804 File Offset: 0x00078A04
		private void RefreshMarkPositions()
		{
			if (this.lightMark == null)
			{
				return;
			}
			if (this.superHeavyMark == null)
			{
				return;
			}
			float d = this.BarWidth * this.LightPercentage;
			float d2 = this.BarWidth * this.SuperHeavyPercentage;
			this.lightMark.anchoredPosition = Vector2.right * d;
			this.superHeavyMark.anchoredPosition = Vector2.right * d2;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0007A878 File Offset: 0x00078A78
		private void RefreshMarkStatus()
		{
			float num = 0f;
			if (this.MaxWeight > 0f)
			{
				num = this.Target.CharacterItem.TotalWeight / this.MaxWeight;
			}
			this.lightMarkToggle.SetToggle(num > this.LightPercentage);
			this.superHeavyMarkToggle.SetToggle(num > this.SuperHeavyPercentage);
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0007A8D8 File Offset: 0x00078AD8
		private void OnTargetChildChanged(Item item)
		{
			this.Animate().Forget();
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0007A8E5 File Offset: 0x00078AE5
		private void OnItemSelectionChanged()
		{
			this.Animate().Forget();
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0007A8F4 File Offset: 0x00078AF4
		private UniTask Animate()
		{
			WeightBarComplex.<Animate>d__33 <Animate>d__;
			<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Animate>d__.<>4__this = this;
			<Animate>d__.<>1__state = -1;
			<Animate>d__.<>t__builder.Start<WeightBarComplex.<Animate>d__33>(ref <Animate>d__);
			return <Animate>d__.<>t__builder.Task;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0007A938 File Offset: 0x00078B38
		private void ResetChangeBars()
		{
			this.positiveBar.DOKill(false);
			this.negativeBar.DOKill(false);
			this.positiveBar.sizeDelta = new Vector2(this.positiveBar.sizeDelta.x, 0f);
			this.negativeBar.sizeDelta = new Vector2(this.negativeBar.sizeDelta.x, 0f);
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0007A9A9 File Offset: 0x00078BA9
		private void ResetMainBar()
		{
			this.mainBar.DOKill(false);
			this.mainBar.sizeDelta = new Vector2(this.mainBar.sizeDelta.x, 0f);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0007A9E0 File Offset: 0x00078BE0
		private UniTask AnimateMainBar(int token)
		{
			WeightBarComplex.<AnimateMainBar>d__37 <AnimateMainBar>d__;
			<AnimateMainBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateMainBar>d__.<>4__this = this;
			<AnimateMainBar>d__.token = token;
			<AnimateMainBar>d__.<>1__state = -1;
			<AnimateMainBar>d__.<>t__builder.Start<WeightBarComplex.<AnimateMainBar>d__37>(ref <AnimateMainBar>d__);
			return <AnimateMainBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0007AA2C File Offset: 0x00078C2C
		private UniTask AnimatePositiveBar(int token)
		{
			WeightBarComplex.<AnimatePositiveBar>d__38 <AnimatePositiveBar>d__;
			<AnimatePositiveBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimatePositiveBar>d__.<>4__this = this;
			<AnimatePositiveBar>d__.token = token;
			<AnimatePositiveBar>d__.<>1__state = -1;
			<AnimatePositiveBar>d__.<>t__builder.Start<WeightBarComplex.<AnimatePositiveBar>d__38>(ref <AnimatePositiveBar>d__);
			return <AnimatePositiveBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0007AA78 File Offset: 0x00078C78
		private UniTask AnimateNegativeBar(int token)
		{
			WeightBarComplex.<AnimateNegativeBar>d__39 <AnimateNegativeBar>d__;
			<AnimateNegativeBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateNegativeBar>d__.<>4__this = this;
			<AnimateNegativeBar>d__.token = token;
			<AnimateNegativeBar>d__.<>1__state = -1;
			<AnimateNegativeBar>d__.<>t__builder.Start<WeightBarComplex.<AnimateNegativeBar>d__39>(ref <AnimateNegativeBar>d__);
			return <AnimateNegativeBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0007AAC3 File Offset: 0x00078CC3
		private void SetupInvalid()
		{
			WeightBarComplex.SetSizeDeltaY(this.mainBar, 0f);
			WeightBarComplex.SetSizeDeltaY(this.positiveBar, 0f);
			WeightBarComplex.SetSizeDeltaY(this.negativeBar, 0f);
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0007AAF8 File Offset: 0x00078CF8
		private static void SetSizeDeltaY(RectTransform rectTransform, float sizeDelta)
		{
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			sizeDelta2.y = sizeDelta;
			rectTransform.sizeDelta = sizeDelta2;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0007AB1B File Offset: 0x00078D1B
		private static float GetSizeDeltaY(RectTransform rectTransform)
		{
			return rectTransform.sizeDelta.y;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0007AB28 File Offset: 0x00078D28
		private float WeightToRectHeight(float weight)
		{
			if (this.MaxWeight <= 0f)
			{
				return 0f;
			}
			float num = weight / this.MaxWeight;
			return this.BarWidth * num;
		}

		// Token: 0x040017C1 RID: 6081
		[SerializeField]
		private CharacterMainControl target;

		// Token: 0x040017C2 RID: 6082
		[SerializeField]
		private RectTransform barArea;

		// Token: 0x040017C3 RID: 6083
		[SerializeField]
		private RectTransform mainBar;

		// Token: 0x040017C4 RID: 6084
		[SerializeField]
		private Graphic mainBarGraphic;

		// Token: 0x040017C5 RID: 6085
		[SerializeField]
		private RectTransform positiveBar;

		// Token: 0x040017C6 RID: 6086
		[SerializeField]
		private RectTransform negativeBar;

		// Token: 0x040017C7 RID: 6087
		[SerializeField]
		private RectTransform lightMark;

		// Token: 0x040017C8 RID: 6088
		[SerializeField]
		private RectTransform superHeavyMark;

		// Token: 0x040017C9 RID: 6089
		[SerializeField]
		private ToggleAnimation lightMarkToggle;

		// Token: 0x040017CA RID: 6090
		[SerializeField]
		private ToggleAnimation superHeavyMarkToggle;

		// Token: 0x040017CB RID: 6091
		[SerializeField]
		private Color superLightColor;

		// Token: 0x040017CC RID: 6092
		[SerializeField]
		private Color lightColor;

		// Token: 0x040017CD RID: 6093
		[SerializeField]
		private Color superHeavyColor;

		// Token: 0x040017CE RID: 6094
		[SerializeField]
		private Color overweightColor;

		// Token: 0x040017CF RID: 6095
		[SerializeField]
		private float animateDuration = 0.1f;

		// Token: 0x040017D0 RID: 6096
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040017D1 RID: 6097
		private float targetRealBarTop;

		// Token: 0x040017D2 RID: 6098
		private int currentToken;
	}
}
