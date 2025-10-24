using System;

// Token: 0x0200006E RID: 110
[Serializable]
public struct ElementFactor
{
	// Token: 0x06000423 RID: 1059 RVA: 0x00012584 File Offset: 0x00010784
	public ElementFactor(ElementTypes _type, float _factor)
	{
		this.elementType = _type;
		this.factor = _factor;
	}

	// Token: 0x04000331 RID: 817
	public ElementTypes elementType;

	// Token: 0x04000332 RID: 818
	public float factor;
}
