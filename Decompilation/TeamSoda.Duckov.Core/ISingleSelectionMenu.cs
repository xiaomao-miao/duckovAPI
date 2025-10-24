using System;

// Token: 0x0200015E RID: 350
public interface ISingleSelectionMenu<EntryType> where EntryType : class
{
	// Token: 0x06000AB2 RID: 2738
	EntryType GetSelection();

	// Token: 0x06000AB3 RID: 2739
	bool SetSelection(EntryType selection);
}
