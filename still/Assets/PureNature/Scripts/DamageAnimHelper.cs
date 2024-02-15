using UnityEngine;

public class DamageAnimHelper : MonoBehaviour
{
    public Animator ani;

    private int damage; 

    public int Damage
    {
        get => damage;
        set
        {
            damage = value;
            UpdateTextMesh(); 
        }
    }

    private TextMesh textMesh;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        textMesh = GetComponent<TextMesh>();
    }

    private void UpdateTextMesh()
    {
        if (textMesh != null)
        {
            textMesh.text = damage.ToString();
            ani.SetTrigger("AttackGo");
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;       
    }

    public void DestroyFunction()
    {
        ObjectPooler.ReturnObject(gameObject);
    }
}