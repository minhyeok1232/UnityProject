using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;

public class FirebaseAuthManager
{
    public static event Action OnLoginSuccess = delegate { };
    public event Action OnLoginFailure = delegate { };

    private static FirebaseAuthManager instance = null;
    public static FirebaseAuthManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseAuthManager();
            }
            return instance;
        }
    }

    private FirebaseAuth auth;  // 로그인, 회원가입 등에 사용
    private FirebaseUser user;  // 인증이 완성된 유저들의 정보 

    public string UserId => user.UserId;

    public Action<bool> LoginState;
    void Start()
    {
        // Firebase Authentication 상태 리스너 추가
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChanged;
        CheckAuthState();
    }
    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnChanged;
    }
    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            {
                if (!signed && user != null)
                {
                    Debug.Log("로그아웃");
                    LoginState?.Invoke(false);
                }

                user = auth.CurrentUser;
                if (signed)
                {
                    Debug.Log("로그인");
                    LoginState?.Invoke(true);
                }
            }
        }
    }

    public void Create(string email2, string password2)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email2, password2).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                // 회원가입 실패 이유 => 이메일 비정상, 비밀번호가 너무 간단, 이미 가입된 이메일 등등...
                Debug.LogError("회원가입 실패: " + task.Exception.ToString());
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.LogError("회원가입 완료");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                OnLoginFailure.Invoke();  // 로그인 실패 이벤트 호출
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            
            OnLoginSuccess.Invoke();
        });
    }

    public void LogOut()
    {
        auth.SignOut();
    }


    void OnDestroy()
    {
        // 필요한 경우 리스너 제거
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }

    void CheckAuthState()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            // 사용자가 로그인 상태
            Debug.Log("User is logged in: " + user.Email);
        }
        else
        {
            // 사용자가 로그아웃 상태 또는 계정이 삭제됨
            Debug.Log("User is logged out or account is deleted");
            // 로그인 화면으로 이동하거나 로그아웃 처리
        }
    }

    void HandleAuthStateChanged(object sender, EventArgs eventArgs)
    {
        CheckAuthState();
    }
}
