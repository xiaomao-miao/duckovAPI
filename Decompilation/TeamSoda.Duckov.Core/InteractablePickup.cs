using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class InteractablePickup : InteractableBase
{
	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001FA09 File Offset: 0x0001DC09
	public DuckovItemAgent ItemAgent
	{
		get
		{
			return this.itemAgent;
		}
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0001FA11 File Offset: 0x0001DC11
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0001FA14 File Offset: 0x0001DC14
	public void OnInit()
	{
		if (this.itemAgent && this.itemAgent.Item && this.sprite)
		{
			this.sprite.sprite = this.itemAgent.Item.Icon;
		}
		this.overrideInteractName = true;
		base.InteractName = this.itemAgent.Item.DisplayNameRaw;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0001FA85 File Offset: 0x0001DC85
	protected override void OnInteractStart(CharacterMainControl character)
	{
		character.PickupItem(this.itemAgent.Item);
		base.StopInteract();
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0001FAA0 File Offset: 0x0001DCA0
	public void Throw(Vector3 direction, float randomAngle)
	{
		this.throwStartPoint = base.transform.position;
		if (!this.rb)
		{
			this.rb = base.gameObject.AddComponent<Rigidbody>();
		}
		this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		this.rb.constraints = RigidbodyConstraints.FreezeRotation;
		if (direction.magnitude < 0.1f)
		{
			direction = Vector3.zero;
		}
		else
		{
			direction.y = 0f;
			direction.Normalize();
			direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-randomAngle, randomAngle) * 0.5f, 0f) * direction;
			direction *= UnityEngine.Random.Range(0.5f, 1f) * 3f;
			direction.y = 2.5f;
		}
		this.rb.velocity = direction;
		this.DestroyRigidbody().Forget();
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0001FB87 File Offset: 0x0001DD87
	protected override void OnDestroy()
	{
		this.destroied = true;
		base.OnDestroy();
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0001FB98 File Offset: 0x0001DD98
	private UniTaskVoid DestroyRigidbody()
	{
		InteractablePickup.<DestroyRigidbody>d__12 <DestroyRigidbody>d__;
		<DestroyRigidbody>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DestroyRigidbody>d__.<>4__this = this;
		<DestroyRigidbody>d__.<>1__state = -1;
		<DestroyRigidbody>d__.<>t__builder.Start<InteractablePickup.<DestroyRigidbody>d__12>(ref <DestroyRigidbody>d__);
		return <DestroyRigidbody>d__.<>t__builder.Task;
	}

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	private DuckovItemAgent itemAgent;

	// Token: 0x040006AA RID: 1706
	public SpriteRenderer sprite;

	// Token: 0x040006AB RID: 1707
	private Rigidbody rb;

	// Token: 0x040006AC RID: 1708
	private Vector3 throwStartPoint;

	// Token: 0x040006AD RID: 1709
	private bool destroied;
}
