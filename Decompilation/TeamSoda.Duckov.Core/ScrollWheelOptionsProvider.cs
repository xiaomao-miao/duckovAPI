using System;

// Token: 0x020001C6 RID: 454
public class ScrollWheelOptionsProvider : OptionsProviderBase
{
	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00037A56 File Offset: 0x00035C56
	public override string Key
	{
		get
		{
			return "Input_ScrollWheelBehaviour";
		}
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x00037A5D File Offset: 0x00035C5D
	public override string GetCurrentOption()
	{
		return ScrollWheelBehaviour.GetDisplayName(ScrollWheelBehaviour.CurrentBehaviour);
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x00037A6C File Offset: 0x00035C6C
	public override string[] GetOptions()
	{
		ScrollWheelBehaviour.Behaviour[] array = (ScrollWheelBehaviour.Behaviour[])Enum.GetValues(typeof(ScrollWheelBehaviour.Behaviour));
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = ScrollWheelBehaviour.GetDisplayName(array[i]);
		}
		return array2;
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x00037AB1 File Offset: 0x00035CB1
	public override void Set(int index)
	{
		ScrollWheelBehaviour.CurrentBehaviour = ((ScrollWheelBehaviour.Behaviour[])Enum.GetValues(typeof(ScrollWheelBehaviour.Behaviour)))[index];
	}
}
