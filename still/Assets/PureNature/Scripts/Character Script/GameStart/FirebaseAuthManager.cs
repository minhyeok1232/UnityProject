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

    private FirebaseAuth auth;  // �α���, ȸ������ � ���
    private FirebaseUser user;  // ������ �ϼ��� �������� ���� 

    public string UserId => user.UserId;

    public Action<bool> LoginState;
    void Start()
    {
        // Firebase Authentication ���� ������ �߰�
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
                    Debug.Log("�α׾ƿ�");
                    LoginState?.Invoke(false);
                }

                user = auth.CurrentUser;
                if (signed)
                {
                    Debug.Log("�α���");
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
                Debug.LogError("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                // ȸ������ ���� ���� => �̸��� ������, ��й�ȣ�� �ʹ� ����, �̹� ���Ե� �̸��� ���...
                Debug.LogError("ȸ������ ����: " + task.Exception.ToString());
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.LogError("ȸ������ �Ϸ�");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                OnLoginFailure.Invoke();  // �α��� ���� �̺�Ʈ ȣ��
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
        // �ʿ��� ��� ������ ����
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }

    void CheckAuthState()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            // ����ڰ� �α��� ����
            Debug.Log("User is logged in: " + user.Email);
        }
        else
        {
            // ����ڰ� �α׾ƿ� ���� �Ǵ� ������ ������
            Debug.Log("User is logged out or account is deleted");
            // �α��� ȭ������ �̵��ϰų� �α׾ƿ� ó��
        }
    }

    void HandleAuthStateChanged(object sender, EventArgs eventArgs)
    {
        CheckAuthState();
    }
}
