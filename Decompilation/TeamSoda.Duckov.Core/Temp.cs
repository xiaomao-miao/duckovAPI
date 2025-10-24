using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class Temp : MonoBehaviour
{
	// Token: 0x06000CB6 RID: 3254 RVA: 0x000353C0 File Offset: 0x000335C0
	private void Calculate(float fps = 100f)
	{
		float num = 1f / fps;
		int num2 = 7;
		float num3 = 3f;
		float num4 = 3f;
		int num5 = 2;
		int i = 15;
		List<float> list = new List<float>();
		List<float> list2 = new List<float>();
		for (float num6 = 0f; num6 <= 100f; num6 += num)
		{
			while (i >= num2)
			{
				i -= num2;
				list.Add(0f);
			}
			for (int j = 0; j < list.Count; j++)
			{
				float num7 = list[j];
				num7 += num;
				if (num7 >= num3)
				{
					list.RemoveAt(j);
					j--;
					list2.Add(0f);
				}
				else
				{
					list[j] = num7;
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				float num8 = list2[k];
				num8 += num;
				while (num8 > num4)
				{
					num8 -= num4;
					i += num5;
				}
				list2[k] = num8;
			}
		}
		Debug.Log(string.Format("{0} {1}", list2.Count, i));
	}
}
