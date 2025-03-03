using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Script
    public PlayerController player;

    public static bool inventoryActivated;                  // inventoryActivated 인벤토리가 활성화 되었는지를 추적하는 정적변수 (초기값은 false로 설정)

    [SerializeField]
    private GameObject go_InventoryBase;                            // 직렬화 go_InventoryBase는 InventoryUI 를 나타낸다.
    [SerializeField]
    private GameObject go_SlotsParent;                              

    private Slot[] slots;                                           // Slot는 20개로 되어있으며, 그것의 배열들을 slots로 나타낸다.

    public Slot[] GetSlots()                                        // public 메소드 추가 (다른 스크립트에서 참고하기 위해서)
    {
        return slots;                                               // GetSlots메소드를 호출하면 해당 slots를 나타낸다.
    }

    void Start()
    {
        CloseInventory();

        slots = go_SlotsParent.GetComponentsInChildren<Slot>();     // 선언한 slots는 Content의 자식 Slot들로 초기화를 하고 시작

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize(this);
        }
    }

    void Update()                                                   // I키를 누를때 마다,
    {
        TryOpenInventory();                                         // 인벤토리의 열고 닫음을 Update해준다.
        RefreshInventoryUI();

        if(inventoryActivated && IsMouseWithinInventoryUI())
        {
            player.canAttack2 = false;
        }
        else
            player.canAttack2 = true;
    }
    public void RefreshInventoryUI()
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemCount > 0) // 아이템 갯수가 0보다 큰 경우
            {
                slot.go_CountImage.SetActive(true);
                slot.text_Count.text = slot.itemCount.ToString();
            }
            else // 아이템이 없는 경우
            {
                slot.go_CountImage.SetActive(false);
                slot.text_Count.text = "0";
            }
        }
    }

    private void TryOpenInventory()                                 // TryOpenInventory 메소드는
    {
        if (Input.GetKeyDown(KeyCode.I))                            // I키를 누르게 되면,
        {
            if (!inventoryActivated)                                 // 만약에, inventoryActivated = True 라면,
                OpenInventory();                                    // OpenInventory()를 실행하며,
            else
                CloseInventory();                                   // False라면 CloseInventory()를 실행한다.
        }
    }

    private void OpenInventory()                                    // OpenInventory() 메소드는, InventoryUI (Panel부터 그 아래까지)
    {
        go_InventoryBase.SetActive(true);                           // SetActive(true)로 켜줘야 하며,
        inventoryActivated = true;
    }

    private void CloseInventory()                                   // CloseInventory() 메소드는, InventoryUI (Panel부터 그 아래까지)
    {   
        go_InventoryBase.SetActive(false);                          // SetActive(false)로 꺼줘야한다.
        inventoryActivated = false;
    }

    public void AcquireItem(Item _item, int _count = 1)             // AcquireItem 메소드는 Item 스크립트의 _item, int형 변수의 _count는 1로 초기화한다)
    {
        if (Item.ItemType.Equipment != _item.itemType)              // Item 스크립트 내 ItemType(enum형)이 장비아이템이 아니라면,
        {
            for (int i = 0; i < slots.Length; i++)                  // 0부터 시작해서, slots.Length(20) 즉 19까지
            {
                if (slots[i].item != null)                          // i번째 slots에 있는 item이 비어있지 않다면,
                {
                    if (slots[i].item.itemName == _item.itemName)   // _item의 itemName을 받아오고,
                    {
                        slots[i].SetSlotCount(_count);              // i번째 slots에 있는 아이템을 SetSlotCount메소드를 실행시킨다. (증가된 _count의 갯수만큼)
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)                      
        {
            if (slots[i].item == null)                              // 근데 만약에 비어있다면,
            {
                slots[i].AddItem(_item, _count);                    // AddItem 메소드를 실행시켜, 그림과 _count ( 0-> 1) 을 보여주게 한다.
                return;
            }
        }
    }

    public void UseItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if(slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].UseSlotCount(_count);
                        return;
                    }
                }
            }
        }
    }
    private bool IsMouseWithinInventoryUI()
    {
        // RectTransform 컴포넌트를 가져옵니다.
        RectTransform inventoryRect = go_InventoryBase.GetComponent<RectTransform>();

        // 마우스의 현재 위치가 인벤토리 UI 안에 있는지 확인
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition);
    }
}