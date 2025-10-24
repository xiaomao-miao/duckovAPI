using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002DB RID: 731
	public class BubblePopperLayout : MiniGameBehaviour
	{
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x00055B16 File Offset: 0x00053D16
		public Vector2 XPositionBorder
		{
			get
			{
				return new Vector2((float)this.xBorder.x * this.BubbleRadius * 2f - this.BubbleRadius, (float)this.xBorder.y * this.BubbleRadius * 2f);
			}
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x00055B58 File Offset: 0x00053D58
		public Vector2 CoordToLocalPosition(Vector2Int coord)
		{
			float bubbleRadius = this.BubbleRadius;
			return new Vector2(((coord.y % 2 != 0) ? bubbleRadius : 0f) + (float)coord.x * bubbleRadius * 2f, (float)coord.y * bubbleRadius * BubblePopperLayout.YOffsetFactor);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00055BA8 File Offset: 0x00053DA8
		public Vector2Int LocalPositionToCoord(Vector2 localPosition)
		{
			float bubbleRadius = this.BubbleRadius;
			int num = Mathf.RoundToInt(localPosition.y / bubbleRadius / BubblePopperLayout.YOffsetFactor);
			float num2 = (num % 2 != 0) ? bubbleRadius : 0f;
			return new Vector2Int(Mathf.RoundToInt((localPosition.x - num2) / bubbleRadius / 2f), num);
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00055BFC File Offset: 0x00053DFC
		public Vector2Int WorldPositionToCoord(Vector2 position)
		{
			Vector3 v = base.transform.worldToLocalMatrix.MultiplyPoint(position);
			return this.LocalPositionToCoord(v);
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00055C30 File Offset: 0x00053E30
		public Vector2Int[] GetAllNeighbourCoords(Vector2Int center, bool includeCenter)
		{
			int num = (center.y % 2 != 0) ? 0 : -1;
			Vector2Int[] result;
			if (includeCenter)
			{
				result = new Vector2Int[]
				{
					new Vector2Int(center.x + num, center.y + 1),
					new Vector2Int(center.x + num + 1, center.y + 1),
					new Vector2Int(center.x - 1, center.y),
					center,
					new Vector2Int(center.x + 1, center.y),
					new Vector2Int(center.x + num, center.y - 1),
					new Vector2Int(center.x + num + 1, center.y - 1)
				};
			}
			else
			{
				result = new Vector2Int[]
				{
					new Vector2Int(center.x + num, center.y + 1),
					new Vector2Int(center.x + num + 1, center.y + 1),
					new Vector2Int(center.x - 1, center.y),
					new Vector2Int(center.x + 1, center.y),
					new Vector2Int(center.x + num, center.y - 1),
					new Vector2Int(center.x + num + 1, center.y - 1)
				};
			}
			return result;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00055DDC File Offset: 0x00053FDC
		public List<Vector2Int> GetAllPassingCoords(Vector2 localOrigin, Vector2 direction, float length)
		{
			float num = this.BubbleRadius * 2f;
			List<Vector2Int> list = new List<Vector2Int>
			{
				this.LocalPositionToCoord(localOrigin)
			};
			if (num > 0f)
			{
				float num2 = -num;
				while (num2 < length)
				{
					num2 += num;
					Vector2 localPosition = localOrigin + num2 * direction;
					Vector2Int center = this.LocalPositionToCoord(localPosition);
					list.AddRange(this.GetAllNeighbourCoords(center, true));
				}
			}
			return list;
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00055E48 File Offset: 0x00054048
		private void OnDrawGizmos()
		{
			float bubbleRadius = this.BubbleRadius;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(new Vector3(this.XPositionBorder.x, 0f), new Vector3(this.XPositionBorder.x, -100f));
			Gizmos.DrawLine(new Vector3(this.XPositionBorder.y, 0f), new Vector3(this.XPositionBorder.y, -100f));
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00055ED4 File Offset: 0x000540D4
		public void GizmosDrawCoord(Vector2Int coord, float ratio)
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawSphere(this.CoordToLocalPosition(coord), this.BubbleRadius * ratio);
			Gizmos.matrix = matrix;
		}

		// Token: 0x04001114 RID: 4372
		[SerializeField]
		private Vector2Int xBorder;

		// Token: 0x04001115 RID: 4373
		public Vector2Int XCoordBorder;

		// Token: 0x04001116 RID: 4374
		public float BubbleRadius = 8f;

		// Token: 0x04001117 RID: 4375
		public static readonly float YOffsetFactor = Mathf.Tan(1.0471976f);

		// Token: 0x04001118 RID: 4376
		[SerializeField]
		private Transform tester;

		// Token: 0x04001119 RID: 4377
		[SerializeField]
		private float distance = 10f;

		// Token: 0x0400111A RID: 4378
		[SerializeField]
		private Vector2Int min;

		// Token: 0x0400111B RID: 4379
		[SerializeField]
		private Vector2Int max;
	}
}
