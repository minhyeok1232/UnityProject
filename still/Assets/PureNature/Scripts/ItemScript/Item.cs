using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]	// New Item 이라는 파일안에 Item을 만들 수 있는 기능
public class Item : ScriptableObject									// 메모리 낭비를 위해 ScriptableObject 인터페이스를 참조한다.
{																		// Component로 붙일 수 없으며, 아이템이 가지는 정보들을 저장한다.
	// String
	public string itemName;                                             // String 형식의 아이템 이름
	public string weaponType;                                           // String 형식의 무기 유형

	// int
	public int IncreasedHealth;										// HP 회복량

	// Enum
	public ItemType itemType;											// Enum 형식의 아이템 타입

	// Sprite
	public Sprite itemImage;											// 3D에 있는 아이템이 인벤토리 2D에 들어갈 아이템 이미지

	// GameObject
	public GameObject itemPrefab;                                       // 아이템들은 게임오브젝트이며, 프리팹을 유지한다.

	// 설명 필드 추가
	[TextArea] 
	public string description;

	// Enum
	public enum ItemType
	{
		Equipment,
		Hp,
		Consumables,
		Etc
	}
}