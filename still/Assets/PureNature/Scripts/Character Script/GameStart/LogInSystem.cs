using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using UnityEngine.EventSystems;

public class LogInSystem : MonoBehaviour
{
    public InputField email;
    public InputField password;

    public InputField email2;
    public InputField password2;

    public Text outputText;
    public Text LogInMessage;

    public Button BtnClose1;
    public Button BtnClose2;

    public Button Lg_Button;

    public GameObject Log;
    public GameObject Cre;
    public GameObject Forgot;

    private DatabaseReference databaseReference;
    public Selectable nextField; // 다음 입력 필드를 지정


    public void Start()
    {
        Log.SetActive(true);
        Cre.SetActive(false);
        Lg_Button.gameObject.SetActive(false);



        FirebaseAuthManager.Instance.LoginState += OnChangedState;
        FirebaseAuthManager.Instance.OnLoginFailure += HandleLoginFailure;  // 실패 이벤트 구독
        FirebaseAuthManager.Instance.Init();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (nextField != null)
            {
                nextField.Select(); // 다음 입력 필드에 포커스
            }
        }

        if(Input.GetKey(KeyCode.Return))
        {
            LogIn();
        }
    }

    public void Next()
    {
        Log.SetActive(false);
        Cre.SetActive(true);
    }

    public void OnChangedState(bool sign)
    {
        outputText.text += FirebaseAuthManager.Instance.UserId;

        SceneManager.LoadScene("Scene_Demo");
    }

    public void HandleLoginFailure()  // 로그인 실패 처리 메서드
    {
        Failure();
    }
    public void Failure()
    {
        LogInMessage.gameObject.SetActive(true);
    }

    public void CloseButton1()
    {
        Log.SetActive(false);
        Lg_Button.gameObject.SetActive(true);
    }
    public void CloseButton2()
    {
        Cre.gameObject.SetActive(false);
        Lg_Button.gameObject.SetActive(true);
    }
    public void OpenButton()
    {
        Log.SetActive(true);
        Lg_Button.gameObject.SetActive(false);
    }


    public void Remember()
    {
        Log.SetActive(true);
        Cre.SetActive(false);
    }

    public void Create()
    {
        string e = email2.text;
        string p = password2.text;

        FirebaseAuthManager.Instance.Create(e, p);

        Log.SetActive(true);
        Cre.SetActive(false);
    }

    public void LogIn()
    {
        FirebaseAuthManager.Instance.Login(email.text, password.text);
    }

    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }
    private void OnDestroy()
    {
        FirebaseAuthManager.Instance.LoginState -= OnChangedState;
        FirebaseAuthManager.Instance.OnLoginFailure -= HandleLoginFailure;  // 실패 이벤트 구독
    }
}
