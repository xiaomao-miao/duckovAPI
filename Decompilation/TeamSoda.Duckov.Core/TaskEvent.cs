using System;

// Token: 0x02000120 RID: 288
public class TaskEvent
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06000981 RID: 2433 RVA: 0x00029664 File Offset: 0x00027864
	// (remove) Token: 0x06000982 RID: 2434 RVA: 0x00029698 File Offset: 0x00027898
	public static event Action<string> OnTaskEvent;

	// Token: 0x06000983 RID: 2435 RVA: 0x000296CB File Offset: 0x000278CB
	public static void EmitTaskEvent(string taskEventKey)
	{
		Action<string> onTaskEvent = TaskEvent.OnTaskEvent;
		if (onTaskEvent == null)
		{
			return;
		}
		onTaskEvent(taskEventKey);
	}
}
