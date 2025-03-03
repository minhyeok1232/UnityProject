using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // SerializedField
    //[SerializeField]
    //private Text text_Count;                            // private 선언 직렬화 Text 변수 타입 text_Count
    public Text text_Count;
    //[SerializeField]
    //private GameObject go_CountImage;                   // private 선언 직렬화 GameObject 변수 타입 go_CountImage : 게임오브젝트의 이미지를 private로 코드에서 수정 불가능
    public GameObject go_CountImage;

    // Item
    public Item item;                                   // Item 스크립트를 item이라고 선언
    public int itemCount;                               // itemCount는 아이템 갯수를 확인하기 위함
    public Image itemImage;                             // itemImage는 Image형식으로됨

    // GameObject
    public GameObject itemDescriptionUI; // 아이템 설명 UI의 참조.
    public GameObject rightClickUI; // 우클릭 UI의 참조.
    public GameObject RemoveUI;     // 아이템 버릴때 Image
    public GameObject RemoveItemImage;
    public GameObject itemPrefab;
    public GameObject Cha;

    // Button
    public Button itemUseButton;
    public Button itemOutButton;

    // Script
    public Inventory inventory;

    // Slot
    private Slot currentSelectedSlot;

    // Text, InputField
    public InputField DropInputField; // 수량 입력을 위한 InputField
    public Text DropText;

    // Vector
    private Vector2 beginPoint;                                                     // 3D 지만, UI는 2D로 구성. UI의 위치를 저장하는 벡터형 변수
    private Vector2 moveBegin;                                                      // 마우스 위치를 저장하는 벡터형 변수
    private Vector2 originalPosition;

    // Start
    public void Start()
    {
        itemDescriptionUI.SetActive(false);             // 게임 시작 시에는 아이템 설명, 우클릭 UI는 보이지 않게 설정한다.    
        rightClickUI.SetActive(false);
        RemoveUI.SetActive(false);
    }

    public void Initialize(Inventory inventory)
    {
        itemUseButton = rightClickUI.transform.Find("ItemUse").GetComponent<Button>();
        itemUseButton.onClick.AddListener(() => OnItemUseClicked(item));
        itemOutButton.onClick.AddListener(() => OnItemOutClicked(item));

        DropInputField.onEndEdit.AddListener(HandleEndEdit);
        DropInputField.onValueChanged.AddListener(OnInputFieldValueChanged); // 값 변경 콜백 함수 등록
    }

    private void SetColor(float _alpha)                 // SetColor 메소드는 0~1사이의 실수형 변수 _alpha에 따라, 투명도를 나타낸다.
    {
        Color color = itemImage.color;                  // 위에 선언한 itemImage의 color를 Color(색상)의 color 변수로 가져온다는 뜻 (color = itemImage.color 라고 이해)
        color.a = _alpha;                               // _alpha값으로, color의 .a(투명도옵션)을 넣어준다.
        itemImage.color = color;                        // SetColor(들어오는 _alpha 변수)에 따라, itemImage.color 전체가 영향에 미치게 된다. 
    }                                                   // 즉, itemImage.color = color; 이 코드로, 알파값에 따라 아이템이미지의 alpha값이 영향을 받는다는 뜻이다.


    public void AddItem(Item _item, int _count = 1)     // AddItem 메소드는, Item 스크립트의 _item과, int 형식으로된 _count를 1로 초기화 시킨 상태에서...
    {
        item = _item;                                   // _item은 item으로 선언
        itemCount = _count;                             // _count는 itemCount로 선언
        itemImage.sprite = item.itemImage;              // item의 itemImage는 sprite형식으로 된 itemImage를 받는다.

        if (item.itemType != Item.ItemType.Equipment)   // item 스크립트의 itemType(enum형식)이 장비아이템이 아니라면 (갯수를 표현해야지)
        {
            go_CountImage.SetActive(true);              // go_CountImage는 켜지게 된다. (갯수를 표현하기 위해)
            text_Count.text = itemCount.ToString();     // go_CountImage 자식의 text형식은, itemCount(_count 먹은만큼의 갯수)를 String()형식으로 표현하게 된다.
        }
        else
        {   
            text_Count.text = "0";                      // 장비아이템이라면?
            go_CountImage.SetActive(false);             // text는 0이되며, 그와 동시에 go_CountImage도 꺼지게 된다. 
        }

        SetColor(1);                                    // 아이템을 먹었기 때문에, 아이템이 보여야하므로, SetColor 메소드의 _alpha값을 1로 표현해서, 불투명하게 만든다.
    }

    public void SetSlotCount(int _count)                // SetSlotCount는 int 형식으로된 _count를 받는다.
    {
        itemCount += _count;                            // itemCount는 AddItem에서 1로 초기화를 시켰고, 아이템을 먹을 때 마다 갯수를 1씩 증가시키기 위한 코드이다.
        text_Count.text = itemCount.ToString();         
    }

    public void UseSlotCount(int _count)
    {
        itemCount -= _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            currentSelectedSlot.ClearSlot();
    }

    private void ClearSlot()                            // 아이템이 비게 된다면?
    {
        item = null;                                    // item은 null이 되어야하고,
        itemCount = 0;                                  // itemCount는 0이 되어야하고,
        itemImage.sprite = null;                        // itemImage.sprite는 null이 되어야하고,
        SetColor(0);                                    // SetColor의 _alpha값을 0으로 만들어서 투명하게 해줘야하고,

        text_Count.text = "0";                          // go_CountImage 자식의 text형식도 0이 되어야하며,
        go_CountImage.SetActive(false);                 // go_CountImage 오브젝트의 불은 꺼져야한다.
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData)          
    {
        if (item != null)
        {
            ShowItemInfo(eventData.position);           // 마우스의 position값을 받아서, ShowItemInfo()메소드 실행
        }
    }

    // Hoverout
    public void OnPointerExit(PointerEventData eventData)
    {
        HideItemInfo();                                 // 마우스가 해당 Slot위치에서 벗어날 시 HideItemInfo() 메소드 실행
    }

    // Click(우클릭)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)     // 아이템이 있는 상황에서 우클릭시 이벤트 실행
        {
            RightClickItem(this, eventData.position, eventData.button);                 // 마우스의 position값을 받고, button값을 받아서, this(해당 슬롯 파악)하기 위한
        }                                                                               // RightClickItem() 메소드 실행
    }

    public void Trash(Slot clickedSlot)
    {
        currentSelectedSlot = clickedSlot; // 현재 선택된 슬롯 저장
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;

            Trash(this);
        }
        else
            return;
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
        else
            return;

        SetColor(0.5f);
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item == null) return;

        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;

        if (itemCount > 1)
        {
            itemImageMethod(item);
            RemoveUI.SetActive(true);
        }
        else
        {
            DropItem(item, 1);
        }

        SetColor(1);
    }


    private void ShowItemInfo(Vector2 position)
    {
        itemDescriptionUI.transform.SetParent(transform);

        RectTransform rectTransform = itemDescriptionUI.GetComponent<RectTransform>();
        rectTransform.position = position;

        Text descriptionText = itemDescriptionUI.transform.Find("ItemText").GetComponent<Text>();
        if (descriptionText != null && item != null)
        {
            descriptionText.text = item.description;
        }

        itemDescriptionUI.SetActive(true);
    }
    private void HideItemInfo()
    {
        // 아이템 정보를 화면에서 숨기는 코드를 작성합니다.
        itemDescriptionUI.SetActive(false);
        rightClickUI.SetActive(false);
    }

    // RightClickItem 메소드의 정의
    public void RightClickItem(Slot clickedSlot, Vector2 position2, PointerEventData.InputButton button)
    {
        // 우클릭 UI를 활성화합니다.
        rightClickUI.SetActive(true);

        rightClickUI.transform.SetParent(transform, false); // 자식으로 설정하고,

        currentSelectedSlot = clickedSlot; // 현재 선택된 슬롯 저장

        RectTransform rectTransform = rightClickUI.GetComponent<RectTransform>(); // rightClickUI에 RectTransform 컴퍼넌트 참조

        rectTransform.position = position2;


        // 아이템 설명 UI를 비활성화합니다.
        itemDescriptionUI.SetActive(false);
    }

    public void OnItemUseClicked(Item _item)
    {
        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.UseSlotCount(1);
            TestSlider.instance.IncreasedValueHealth(_item.IncreasedHealth);

            currentSelectedSlot = null; // OnItemOutClicked메소드완벽히구현한후 이코드 그대로 넣어줘야함
        }

        rightClickUI?.SetActive(false);
    }

    public void OnItemOutClicked(Item _item)
    {
        if (currentSelectedSlot != null)
        {
            if (itemCount > 1)
            {
                itemImageMethod(item);
                RemoveUI.SetActive(true);
            }
            else
            {
                DropItem(item, 1);
            }
        }

        rightClickUI?.SetActive(false);

    }

    public void itemImageMethod(Item _item)
    {
        Image ImageComponent = RemoveItemImage.GetComponent<Image>();
        ImageComponent.sprite = itemImage.sprite;
    }


    private void HandleEndEdit(string inputText)                                        // 
    {
        if (currentSelectedSlot != null)                                                // 내가 사용하는 슬롯이 있으면
        {
            DropInputField = GetComponentInChildren<InputField>();                      //

            int count;                                                  
            count = int.Parse(inputText);                                               // inputText를 int형으로 변환하는 변수 count
            DropItem(item, count);                                                      // 아이템 떨어뜨리는 메소드

            RemoveUI.SetActive(false);                                                  // 아이템을 뿌렸으니, RemoveUI는 가려준다.
        }
    }

    private void DropItem(Item item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject droppedItem = Instantiate(item.itemPrefab, Cha.transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            Rigidbody rb = droppedItem.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
        currentSelectedSlot.UseSlotCount(count);                                        // 현재 슬롯에서 뿌린 개수만큼 아이템 개수를 줄이기 위한 메소드

        Trash(this);

        currentSelectedSlot = null;
    }
    private void OnInputFieldValueChanged(string value)                                 // 수량 조절해주는 메소드
    {
        if (currentSelectedSlot != null)
        {
            int enteredValue;                                                           // String -> Int형으로 변환해주는 정수값

            if (int.TryParse(value, out enteredValue))                                  // 입력한 값이 정수로 변환이 가능한지
            {
                if (enteredValue > itemCount)                                           // 정수값이 itemCount 보다 크게 되면,
                {
                    DropInputField.text = itemCount.ToString();                         // InputField의 값을 아이템 갯수로 변경
                }
            }
        }
    }
}
