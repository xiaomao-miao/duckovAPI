using System;
using UnityEngine;

namespace Duckov.Splines
{
	// Token: 0x02000329 RID: 809
	public class Bevel
	{
		// Token: 0x06001B55 RID: 6997 RVA: 0x00062F94 File Offset: 0x00061194
		public static Vector3[] Evaluate(Vector3 cur, Vector3 prev, Vector3 next, int step, float offset, out Vector3 o, out Vector3 axis, float protectionOffset = 0.1f, bool useProtectionOffset = false, float clipDistance = 3.4028235E+38f)
		{
			if (offset > clipDistance)
			{
				offset = clipDistance;
			}
			Vector3 vector = cur + (prev - cur).normalized * offset;
			Vector3 vector2 = cur + (next - cur).normalized * offset;
			float num = offset;
			float num2 = (vector2 - vector).magnitude / 2f;
			Vector3 vector3 = (vector + vector2) / 2f - cur;
			float magnitude = vector3.magnitude;
			float num3 = Mathf.Asin(num2 / num);
			float num4 = 1.5707964f - num3;
			float num5 = num2 / Mathf.Tan(num4);
			Vector3 vector4 = cur + vector3.normalized * (num5 + magnitude);
			o = vector4;
			Vector3 vector5 = vector - vector4;
			Vector3 rhs = vector2 - vector4;
			float magnitude2 = vector5.magnitude;
			axis = Vector3.Cross(vector5, rhs);
			int num6 = step + 2;
			float num7 = num4 * 2f / (float)(num6 - 1) / 3.1415927f * 180f;
			Vector3[] array = new Vector3[num6];
			for (int i = 0; i < num6; i++)
			{
				Quaternion rotation = Quaternion.AngleAxis(num7 * (float)i, axis);
				array[i] = vector4 + rotation * vector5;
			}
			if (useProtectionOffset)
			{
				Vector3[] array2 = new Vector3[num6 + 2];
				array.CopyTo(array2, 1);
				array2[0] = vector + (vector - cur).normalized * protectionOffset;
				array2[array2.Length - 1] = vector2 + (vector2 - cur).normalized * protectionOffset;
				return array2;
			}
			return array;
		}
	}
}
