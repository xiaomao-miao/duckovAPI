using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.MiniMaps
{
	// Token: 0x02000275 RID: 629
	public interface IPointOfInterest
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x000497B3 File Offset: 0x000479B3
		int OverrideScene
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x000497B6 File Offset: 0x000479B6
		Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x000497B9 File Offset: 0x000479B9
		Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x000497C0 File Offset: 0x000479C0
		string DisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x000497C3 File Offset: 0x000479C3
		Color ShadowColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x000497CA File Offset: 0x000479CA
		float ShadowDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x000497D1 File Offset: 0x000479D1
		bool IsArea
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x000497D4 File Offset: 0x000479D4
		float AreaRadius
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x000497DB File Offset: 0x000479DB
		bool HideIcon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x000497DE File Offset: 0x000479DE
		float ScaleFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x000497E5 File Offset: 0x000479E5
		void NotifyClicked(PointerEventData eventData)
		{
		}
	}
}
