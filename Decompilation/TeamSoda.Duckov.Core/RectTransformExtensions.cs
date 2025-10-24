using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public static class RectTransformExtensions
{
	// Token: 0x0600052B RID: 1323 RVA: 0x0001757A File Offset: 0x0001577A
	public static Camera GetUICamera()
	{
		return null;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00017580 File Offset: 0x00015780
	public static void MatchWorldPosition(this RectTransform rectTransform, Vector3 worldPosition, Vector3 worldSpaceOffset = default(Vector3))
	{
		RectTransform rectTransform2 = rectTransform.parent as RectTransform;
		if (rectTransform2 == null)
		{
			return;
		}
		worldPosition += worldSpaceOffset;
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform2, screenPoint, RectTransformExtensions.GetUICamera(), out v);
		rectTransform.localPosition = v;
	}
}
