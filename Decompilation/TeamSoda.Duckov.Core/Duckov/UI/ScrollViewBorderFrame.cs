using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C4 RID: 964
	public class ScrollViewBorderFrame : MonoBehaviour
	{
		// Token: 0x06002317 RID: 8983 RVA: 0x0007ACA2 File Offset: 0x00078EA2
		private void OnEnable()
		{
			this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.Refresh));
			UniTask.Void(delegate()
			{
				ScrollViewBorderFrame.<<OnEnable>b__8_0>d <<OnEnable>b__8_0>d;
				<<OnEnable>b__8_0>d.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<<OnEnable>b__8_0>d.<>4__this = this;
				<<OnEnable>b__8_0>d.<>1__state = -1;
				<<OnEnable>b__8_0>d.<>t__builder.Start<ScrollViewBorderFrame.<<OnEnable>b__8_0>d>(ref <<OnEnable>b__8_0>d);
				return <<OnEnable>b__8_0>d.<>t__builder.Task;
			});
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0007ACD1 File Offset: 0x00078ED1
		private void OnDisable()
		{
			this.scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.Refresh));
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0007ACEF File Offset: 0x00078EEF
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0007ACF8 File Offset: 0x00078EF8
		private void Refresh(Vector2 scrollPos)
		{
			RectTransform viewport = this.scrollRect.viewport;
			RectTransform content = this.scrollRect.content;
			Rect rect = viewport.rect;
			Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, content);
			float num = bounds.max.y - rect.max.y + this.extendOffset;
			float num2 = rect.min.y - bounds.min.y + this.extendOffset;
			float num3 = rect.min.x - bounds.min.x + this.extendOffset;
			float num4 = bounds.max.x - rect.max.x + this.extendOffset;
			float alpha = Mathf.Lerp(0f, this.maxAlpha, num / this.extendThreshold);
			float alpha2 = Mathf.Lerp(0f, this.maxAlpha, num2 / this.extendThreshold);
			float alpha3 = Mathf.Lerp(0f, this.maxAlpha, num3 / this.extendThreshold);
			float alpha4 = Mathf.Lerp(0f, this.maxAlpha, num4 / this.extendThreshold);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.upGraphic, alpha);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.downGraphic, alpha2);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.leftGraphic, alpha3);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.rightGraphic, alpha4);
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0007AE50 File Offset: 0x00079050
		private void Refresh()
		{
			if (this.scrollRect == null)
			{
				return;
			}
			this.Refresh(this.scrollRect.normalizedPosition);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0007AED4 File Offset: 0x000790D4
		[CompilerGenerated]
		internal static void <Refresh>g__SetAlpha|11_0(Graphic graphic, float alpha)
		{
			if (graphic == null)
			{
				return;
			}
			Color color = graphic.color;
			color.a = alpha;
			graphic.color = color;
		}

		// Token: 0x040017D6 RID: 6102
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x040017D7 RID: 6103
		[Range(0f, 1f)]
		[SerializeField]
		private float maxAlpha = 1f;

		// Token: 0x040017D8 RID: 6104
		[SerializeField]
		private float extendThreshold = 10f;

		// Token: 0x040017D9 RID: 6105
		[SerializeField]
		private float extendOffset;

		// Token: 0x040017DA RID: 6106
		[SerializeField]
		private Graphic upGraphic;

		// Token: 0x040017DB RID: 6107
		[SerializeField]
		private Graphic downGraphic;

		// Token: 0x040017DC RID: 6108
		[SerializeField]
		private Graphic leftGraphic;

		// Token: 0x040017DD RID: 6109
		[SerializeField]
		private Graphic rightGraphic;
	}
}
