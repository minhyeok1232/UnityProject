using System.Collections;
using UnityEngine;
using UnityEngine.UI;                               // UI�� ǥ���ϱ� ������ UnityEngine.UI namespace �ʿ�

public class TextAnimation : MonoBehaviour
{
    // Text
    public Text titleText;                          // "������ ���� ����"
    public Text subTitleText;                       // "������ ����"

    // String
    public string fullText = "������ ���� ����:";
    public string nextText = "������ ����";
    
    // Instance
    public float delay = 0.1f;                      // Typing ȿ���� 0.1�ʸ��� ����ȴ�.

    // Start
    private void Start()
    {
        titleText.fontSize = 240;                   // ū Text�� �۾� ũ�� 240
        subTitleText.fontSize = 175;                // ���� Text�� �۾� ũ�� 175
        subTitleText.text = "";                     // ó���� subTitle�� �ʱ�ȭ �Ѵ�. (������ ���߿�)

        StartCoroutine(ShowText());                 // ShowText Coroutine �� �����Ѵ�.
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)     // i�� 0���� �����Ͽ�, string������ fullText�� ���̸�ŭ �ݺ��ϸ�,
        {
            titleText.text = fullText.Substring(0, i); // UI�� Text�� ǥ���Ǵ� titleText.txt�� fullText�� 0��° ���ڿ����� i��°�� ���ڿ����� �����´�.
            yield return new WaitForSeconds(delay);    // i���� �����Կ� ���� ��ȭ�Ǹ�, 0.1�ʸ��� ��ȣ���� ��Ų��.
        }

        yield return new WaitForSeconds(1f);           // ����, titleText�� ��� ����� �ǰ���, 1�ʵڿ� subTitleText�� ����ϰ� �ȴ�.

        for (int j = 0; j <= nextText.Length; j++)
        {
            subTitleText.text = nextText.Substring(0, j);
            yield return new WaitForSeconds(delay);
        }
    }
}