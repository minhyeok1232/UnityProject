using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
	// SerializedField
	[SerializeField]
	private float range;                            // 습득 가능한 최대의 거리 

	[SerializeField]
	private LayerMask layerMask;                    // 특정 Layer에 대해서만 습득이 가능하다.

	[SerializeField]
	private Text actionText;						// UI창에 띄워줄 Text

	[SerializeField]
	private Inventory theInventory;                 // 아이템을 습득 한 이후, 인벤토리에 아이템을 가져와야 한다.

	// Script
	public MainScript main;


	// boolean
	private bool pickupActivated = false;			// 아이템 습득 가능여부

	// Raycast
	private RaycastHit hitInfo;						// 충돌체 정보 저장	


	// Update
	void Update()
	{
		CheckItem();								// 모든 상황에서, CheckItem() 메소드를 확인한다.
		TryAction();								// "E"키의 입력을 검사한다.
	}
	
	// Method
	private void TryAction()
	{
		if (Input.GetKeyDown(KeyCode.E))			// "E"키가 들어오게 된다면,
		{
			CheckItem();							// Item 이 사정거리 안에 존재하는지 검사한다.
			CanPickUp();							// 아이템을 줍는다.
		}
	}

	public void CanPickUp()
	{
		if (pickupActivated)						// 아이템을 주울 수 있는 상태라면,
		{
			theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);	// Inventory 스크립트에 AcquireItem 메소드를 실행시켜준다. 
			Destroy(hitInfo.transform.gameObject);	// 3D 에 보이는 hitInfo에 저장된 아이템을 없애주며,
			InfoDisappear();                        // InfoDisappear() 메소드를 실행 시켜준다.

			// 아이템을 습득하였으므로, 퀘스트 업데이트 메소드를 호출
			string itemName = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName;

			QuestProgress progress = main.QuestProgressing[1];
			if (progress.questStatus == QuestProgress.Status.Proceeding)
			{
				main.OnQuestProgressed(1, QuestInfo.QuestUpdateType.CollectItem, itemName);
			}
		}
	}

	private void CheckItem()	
	{
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))				
		{
			if (hitInfo.transform.tag == "Item")
			{
				ItemInfoAppear();
			}
		}
		else
			InfoDisappear();
	}

	private void ItemInfoAppear()
	{
		pickupActivated = true;								// boolean 형 아이템습득 가능여부의 변수를 true 로 설정하고
		actionText.gameObject.SetActive(true);				// UI 내 Text를 활성화 하기 위하여, actionText의 SetActive를 true로 변경
		actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 줍기 " + "<color=yellow>" + "(E)" + "</color>";
	}														// 이후, hitInfo.transform. -> 레이캐스트를 쏜 오브젝트에 붙어있는 ItemPickUp 스크립트의 컴퍼넌트를 참조하여
															// 그 아이템에 대한 정보를 가져온다는 뜻이다.
	private void InfoDisappear()
	{
		pickupActivated = false;
		actionText.gameObject.SetActive(false);
	}
}
