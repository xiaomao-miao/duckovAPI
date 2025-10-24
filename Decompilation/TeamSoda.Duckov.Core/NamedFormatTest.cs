using System;
using System.Diagnostics;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class NamedFormatTest : MonoBehaviour
{
	// Token: 0x06000947 RID: 2375 RVA: 0x00028FB0 File Offset: 0x000271B0
	private void Test()
	{
		string message = "";
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < this.loopCount; i++)
		{
			message = this.format.Format(this.content);
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time Consumed 1:" + stopwatch.ElapsedMilliseconds.ToString());
		stopwatch = Stopwatch.StartNew();
		for (int j = 0; j < this.loopCount; j++)
		{
			message = string.Format(this.format2, this.content.textA, this.content.textB);
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time Consumed 2:" + stopwatch.ElapsedMilliseconds.ToString());
		UnityEngine.Debug.Log(message);
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0002907C File Offset: 0x0002727C
	private void Test2()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		string message = this.format.Format(new
		{
			this.content.textA,
			this.content.textB
		});
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time Consumed:" + stopwatch.ElapsedMilliseconds.ToString());
		UnityEngine.Debug.Log(message);
	}

	// Token: 0x04000842 RID: 2114
	public string format = "Displaying {textA} {textB}";

	// Token: 0x04000843 RID: 2115
	public string format2 = "Displaying {0} {1}";

	// Token: 0x04000844 RID: 2116
	public NamedFormatTest.Content content;

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	private int loopCount = 100;

	// Token: 0x02000494 RID: 1172
	[Serializable]
	public struct Content
	{
		// Token: 0x04001BD3 RID: 7123
		public string textA;

		// Token: 0x04001BD4 RID: 7124
		public string textB;
	}
}
