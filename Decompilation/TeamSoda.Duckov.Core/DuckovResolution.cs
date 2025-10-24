using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
[Serializable]
public struct DuckovResolution
{
	// Token: 0x06000DAB RID: 3499 RVA: 0x00038194 File Offset: 0x00036394
	public override bool Equals(object obj)
	{
		if (obj is DuckovResolution)
		{
			DuckovResolution duckovResolution = (DuckovResolution)obj;
			if (duckovResolution.height == this.height && duckovResolution.width == this.width)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x000381CF File Offset: 0x000363CF
	public override string ToString()
	{
		return string.Format("{0} x {1}", this.width, this.height);
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x000381F1 File Offset: 0x000363F1
	public DuckovResolution(Resolution res)
	{
		this.height = res.height;
		this.width = res.width;
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x0003820D File Offset: 0x0003640D
	public DuckovResolution(int _width, int _height)
	{
		this.height = _height;
		this.width = _width;
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00038220 File Offset: 0x00036420
	public bool CheckRotioFit(DuckovResolution newRes, DuckovResolution defaultRes)
	{
		float num = (float)newRes.height / (float)newRes.width;
		return Mathf.Abs((float)defaultRes.height - num * (float)defaultRes.width) <= 2f;
	}

	// Token: 0x04000B92 RID: 2962
	public int width;

	// Token: 0x04000B93 RID: 2963
	public int height;
}
