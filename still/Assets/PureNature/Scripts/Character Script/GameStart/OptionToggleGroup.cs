using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class OptionalToggleGroup : MonoBehaviour
{
    ToggleGroup toggleGroup;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        toggleGroup.allowSwitchOff = true;  // 이 부분이 중요합니다.
    }
}