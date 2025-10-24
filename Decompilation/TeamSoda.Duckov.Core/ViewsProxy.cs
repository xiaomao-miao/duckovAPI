using System;
using Duckov.BlackMarkets.UI;
using Duckov.Crops;
using Duckov.Crops.UI;
using Duckov.Endowment.UI;
using Duckov.MasterKeys.UI;
using Duckov.MiniGames;
using Duckov.MiniMaps.UI;
using Duckov.Quests.UI;
using Duckov.UI;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class ViewsProxy : MonoBehaviour
{
	// Token: 0x06000953 RID: 2387 RVA: 0x000291E9 File Offset: 0x000273E9
	public void ShowInventoryView()
	{
		if (LevelManager.Instance.IsBaseLevel && PlayerStorage.Instance)
		{
			PlayerStorage.Instance.InteractableLootBox.InteractWithMainCharacter();
			return;
		}
		InventoryView.Show();
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00029218 File Offset: 0x00027418
	public void ShowQuestView()
	{
		QuestView.Show();
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0002921F File Offset: 0x0002741F
	public void ShowMapView()
	{
		MiniMapView.Show();
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00029226 File Offset: 0x00027426
	public void ShowKeyView()
	{
		MasterKeysView.Show();
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0002922D File Offset: 0x0002742D
	public void ShowPlayerStats()
	{
		PlayerStatsView.Instance.Open(null);
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0002923A File Offset: 0x0002743A
	public void ShowEndowmentView()
	{
		EndowmentSelectionPanel.Show();
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x00029241 File Offset: 0x00027441
	public void ShowMapSelectionView()
	{
		MapSelectionView.Instance.Open(null);
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0002924E File Offset: 0x0002744E
	public void ShowRepairView()
	{
		ItemRepairView.Instance.Open(null);
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0002925B File Offset: 0x0002745B
	public void ShowFormulasIndexView()
	{
		FormulasIndexView.Show();
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00029262 File Offset: 0x00027462
	public void ShowBitcoinView()
	{
		BitcoinMinerView.Show();
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00029269 File Offset: 0x00027469
	public void ShowStorageDock()
	{
		StorageDock.Show();
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00029270 File Offset: 0x00027470
	public void ShowBlackMarket_Demands()
	{
		BlackMarketView.Show(BlackMarketView.Mode.Demand);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00029278 File Offset: 0x00027478
	public void ShowBlackMarket_Supplies()
	{
		BlackMarketView.Show(BlackMarketView.Mode.Supply);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00029280 File Offset: 0x00027480
	public void ShowSleepView()
	{
		SleepView.Show();
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00029287 File Offset: 0x00027487
	public void ShowATMView()
	{
		ATMView.Show();
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0002928E File Offset: 0x0002748E
	public void ShowDecomposeView()
	{
		ItemDecomposeView.Show();
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00029295 File Offset: 0x00027495
	public void ShowGardenView(Garden garnden)
	{
		GardenView.Show(garnden);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0002929D File Offset: 0x0002749D
	public void ShowGamingConsoleView(GamingConsole console)
	{
		GamingConsoleView.Show(console);
	}
}
