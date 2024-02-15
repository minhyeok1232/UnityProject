using System.Collections;
using UnityEngine;
using UnityEngine.UI;                               // UI를 표시하기 때문에 UnityEngine.UI namespace 필요

public class TextAnimation : MonoBehaviour
{
    // Text
    public Text titleText;                          // "숲속의 작은 전사"
    public Text subTitleText;                       // "모험의 시작"

    // String
    public string fullText = "숲속의 작은 전사:";
    public string nextText = "모험의 시작";
    
    // Instance
    public float delay = 0.1f;                      // Typing 효과는 0.1초마다 진행된다.

    // Start
    private void Start()
    {
        titleText.fontSize = 240;                   // 큰 Text의 글씨 크기 240
        subTitleText.fontSize = 175;                // 작은 Text의 글씨 크기 175
        subTitleText.text = "";                     // 처음에 subTitle을 초기화 한다. (이유는 나중에)

        StartCoroutine(ShowText());                 // ShowText Coroutine 을 시작한다.
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)     // i는 0부터 시작하여, string형식의 fullText의 길이만큼 반복하며,
        {
            titleText.text = fullText.Substring(0, i); // UI의 Text로 표현되는 titleText.txt는 fullText의 0번째 문자열부터 i번째의 문자열까지 가져온다.
            yield return new WaitForSeconds(delay);    // i값이 증가함에 따라 변화되며, 0.1초마다 재호출을 시킨다.
        }

        yield return new WaitForSeconds(1f);           // 이후, titleText가 모두 출력이 되고나면, 1초뒤에 subTitleText를 출력하게 된다.

        for (int j = 0; j <= nextText.Length; j++)
        {
            subTitleText.text = nextText.Substring(0, j);
            yield return new WaitForSeconds(delay);
        }
    }
}