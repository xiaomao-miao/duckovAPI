using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000097 RID: 151
public class StrJson
{
	// Token: 0x06000525 RID: 1317 RVA: 0x000173D8 File Offset: 0x000155D8
	private StrJson(params string[] contentPairs)
	{
		this.entries = new List<StrJson.Entry>();
		for (int i = 0; i < contentPairs.Length - 1; i += 2)
		{
			this.entries.Add(new StrJson.Entry(contentPairs[i], contentPairs[i + 1]));
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001741E File Offset: 0x0001561E
	public StrJson Add(string key, string value)
	{
		this.entries.Add(new StrJson.Entry(key, value));
		return this;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00017433 File Offset: 0x00015633
	public static StrJson Create(params string[] contentPairs)
	{
		return new StrJson(contentPairs);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001743C File Offset: 0x0001563C
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{");
		for (int i = 0; i < this.entries.Count; i++)
		{
			StrJson.Entry entry = this.entries[i];
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(string.Concat(new string[]
			{
				"\"",
				entry.key,
				"\":\"",
				entry.value,
				"\""
			}));
		}
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	// Token: 0x040004A7 RID: 1191
	public List<StrJson.Entry> entries;

	// Token: 0x02000448 RID: 1096
	public struct Entry
	{
		// Token: 0x0600262C RID: 9772 RVA: 0x0008525A File Offset: 0x0008345A
		public Entry(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x04001AA5 RID: 6821
		public string key;

		// Token: 0x04001AA6 RID: 6822
		public string value;
	}
}
