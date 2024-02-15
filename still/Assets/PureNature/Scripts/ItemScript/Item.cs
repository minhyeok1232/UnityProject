using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]	// New Item �̶�� ���Ͼȿ� Item�� ���� �� �ִ� ���
public class Item : ScriptableObject									// �޸� ���� ���� ScriptableObject �������̽��� �����Ѵ�.
{																		// Component�� ���� �� ������, �������� ������ �������� �����Ѵ�.
	// String
	public string itemName;                                             // String ������ ������ �̸�
	public string weaponType;                                           // String ������ ���� ����

	// int
	public int IncreasedHealth;										// HP ȸ����

	// Enum
	public ItemType itemType;											// Enum ������ ������ Ÿ��

	// Sprite
	public Sprite itemImage;											// 3D�� �ִ� �������� �κ��丮 2D�� �� ������ �̹���

	// GameObject
	public GameObject itemPrefab;                                       // �����۵��� ���ӿ�����Ʈ�̸�, �������� �����Ѵ�.

	// ���� �ʵ� �߰�
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