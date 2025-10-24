using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Sirenix.Utilities;
using Steamworks;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200026D RID: 621
	public class SteamWorkshopManager : MonoBehaviour
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x00048290 File Offset: 0x00046490
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x00048297 File Offset: 0x00046497
		public static SteamWorkshopManager Instance { get; private set; }

		// Token: 0x0600135C RID: 4956 RVA: 0x0004829F File Offset: 0x0004649F
		private void Awake()
		{
			SteamWorkshopManager.Instance = this;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000482A7 File Offset: 0x000464A7
		private void OnEnable()
		{
			ModManager.Rescan();
			this.SendQueryDetailsRequest();
			ModManager.OnScan += this.OnScanMods;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000482C5 File Offset: 0x000464C5
		private void OnDisable()
		{
			ModManager.OnScan -= this.OnScanMods;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000482D8 File Offset: 0x000464D8
		private void OnScanMods(List<ModInfo> list)
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			foreach (SteamUGCDetails_t steamUGCDetails_t in SteamWorkshopManager.ugcDetailsCache)
			{
				PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
				EItemState itemState = (EItemState)SteamUGC.GetItemState(nPublishedFileId);
				ulong num;
				string text;
				uint num2;
				if ((itemState | EItemState.k_EItemStateInstalled) == itemState && SteamUGC.GetItemInstallInfo(nPublishedFileId, out num, out text, 1024U, out num2))
				{
					ModInfo item;
					if (!ModManager.TryProcessModFolder(text, out item, true, nPublishedFileId.m_PublishedFileId))
					{
						Debug.LogError("Mod processing failed! \nPath:" + text);
					}
					else
					{
						list.Add(item);
					}
				}
			}
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00048380 File Offset: 0x00046580
		public void SendQueryDetailsRequest()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			if (this.CRSteamUGCQueryCompleted == null)
			{
				this.CRSteamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompleted));
			}
			HashSet<PublishedFileId_t> hashSet = new HashSet<PublishedFileId_t>();
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(array, numSubscribedItems);
			hashSet.AddRange(array);
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				if (modInfo.publishedFileId != 0UL)
				{
					hashSet.Add((PublishedFileId_t)modInfo.publishedFileId);
				}
			}
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(SteamUGC.CreateQueryUGCDetailsRequest(hashSet.ToArray<PublishedFileId_t>(), (uint)hashSet.Count));
			this.CRSteamUGCQueryCompleted.Set(hAPICall, null);
			new StringBuilder();
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0004845C File Offset: 0x0004665C
		private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t completed, bool bIOFailure)
		{
			if (bIOFailure)
			{
				Debug.LogError("Steam UGC Query failed", base.gameObject);
				ModManager.Instance.ScanAndActivateMods();
				return;
			}
			UGCQueryHandle_t handle = completed.m_handle;
			uint unNumResultsReturned = completed.m_unNumResultsReturned;
			for (uint num = 0U; num < unNumResultsReturned; num += 1U)
			{
				SteamUGCDetails_t item;
				SteamUGC.GetQueryUGCResult(handle, num, out item);
				SteamWorkshopManager.ugcDetailsCache.Add(item);
			}
			SteamUGC.ReleaseQueryUGCRequest(handle);
			ModManager.Instance.ScanAndActivateMods();
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x000484C8 File Offset: 0x000466C8
		public UniTask<PublishedFileId_t> RequestNewWorkshopItemID()
		{
			SteamWorkshopManager.<RequestNewWorkshopItemID>d__14 <RequestNewWorkshopItemID>d__;
			<RequestNewWorkshopItemID>d__.<>t__builder = AsyncUniTaskMethodBuilder<PublishedFileId_t>.Create();
			<RequestNewWorkshopItemID>d__.<>4__this = this;
			<RequestNewWorkshopItemID>d__.<>1__state = -1;
			<RequestNewWorkshopItemID>d__.<>t__builder.Start<SteamWorkshopManager.<RequestNewWorkshopItemID>d__14>(ref <RequestNewWorkshopItemID>d__);
			return <RequestNewWorkshopItemID>d__.<>t__builder.Task;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0004850B File Offset: 0x0004670B
		private void OnCreateItemResult(CreateItemResult_t result, bool bIOFailure)
		{
			Debug.Log("Creat Item Result Fired A");
			this.createItemResultFired = true;
			this.createItemResult = result;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x00048525 File Offset: 0x00046725
		// (set) Token: 0x06001365 RID: 4965 RVA: 0x0004852C File Offset: 0x0004672C
		public static ulong punBytesProcess { get; private set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x00048534 File Offset: 0x00046734
		// (set) Token: 0x06001367 RID: 4967 RVA: 0x0004853B File Offset: 0x0004673B
		public static ulong punBytesTotal { get; private set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x00048543 File Offset: 0x00046743
		public static float UploadingProgress
		{
			get
			{
				return (float)(SteamWorkshopManager.punBytesProcess / SteamWorkshopManager.punBytesTotal);
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x00048555 File Offset: 0x00046755
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0004855D File Offset: 0x0004675D
		public bool UploadSucceed { get; private set; }

		// Token: 0x0600136B RID: 4971 RVA: 0x00048568 File Offset: 0x00046768
		public UniTask<bool> UploadWorkshopItem(string path, string changeNote = "Unknown")
		{
			SteamWorkshopManager.<UploadWorkshopItem>d__32 <UploadWorkshopItem>d__;
			<UploadWorkshopItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<UploadWorkshopItem>d__.<>4__this = this;
			<UploadWorkshopItem>d__.path = path;
			<UploadWorkshopItem>d__.changeNote = changeNote;
			<UploadWorkshopItem>d__.<>1__state = -1;
			<UploadWorkshopItem>d__.<>t__builder.Start<SteamWorkshopManager.<UploadWorkshopItem>d__32>(ref <UploadWorkshopItem>d__);
			return <UploadWorkshopItem>d__.<>t__builder.Task;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x000485BC File Offset: 0x000467BC
		public static bool IsOwner(ModInfo info)
		{
			if (!SteamManager.Initialized)
			{
				return false;
			}
			if (info.publishedFileId == 0UL)
			{
				return false;
			}
			foreach (SteamUGCDetails_t steamUGCDetails_t in SteamWorkshopManager.ugcDetailsCache)
			{
				if (steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId == info.publishedFileId)
				{
					return steamUGCDetails_t.m_ulSteamIDOwner == SteamUser.GetSteamID().m_SteamID;
				}
			}
			return false;
		}

		// Token: 0x04000E62 RID: 3682
		private CallResult<SteamUGCQueryCompleted_t> CRSteamUGCQueryCompleted;

		// Token: 0x04000E63 RID: 3683
		private CallResult<CreateItemResult_t> CRCreateItemResult;

		// Token: 0x04000E64 RID: 3684
		private UGCQueryHandle_t activeQueryHandle;

		// Token: 0x04000E65 RID: 3685
		private static List<SteamUGCDetails_t> ugcDetailsCache = new List<SteamUGCDetails_t>();

		// Token: 0x04000E66 RID: 3686
		private bool createItemResultFired;

		// Token: 0x04000E67 RID: 3687
		private CreateItemResult_t createItemResult;
	}
}
