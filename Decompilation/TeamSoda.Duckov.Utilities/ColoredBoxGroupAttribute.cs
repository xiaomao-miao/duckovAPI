using System;
using Sirenix.OdinInspector;

namespace Duckov.Utilities
{
	// Token: 0x02000003 RID: 3
	public class ColoredBoxGroupAttribute : PropertyGroupAttribute
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public ColoredBoxGroupAttribute(string path) : base(path)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C9 File Offset: 0x000002C9
		public ColoredBoxGroupAttribute(string path, float r, float g, float b, float a = 1f) : base(path)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		protected override void CombineValuesWith(PropertyGroupAttribute other)
		{
			ColoredBoxGroupAttribute coloredBoxGroupAttribute = (ColoredBoxGroupAttribute)other;
			this.R = Math.Max(coloredBoxGroupAttribute.R, this.R);
			this.G = Math.Max(coloredBoxGroupAttribute.G, this.G);
			this.B = Math.Max(coloredBoxGroupAttribute.B, this.B);
			this.A = Math.Max(coloredBoxGroupAttribute.A, this.A);
		}

		// Token: 0x04000001 RID: 1
		public float R;

		// Token: 0x04000002 RID: 2
		public float G;

		// Token: 0x04000003 RID: 3
		public float B;

		// Token: 0x04000004 RID: 4
		public float A;
	}
}
