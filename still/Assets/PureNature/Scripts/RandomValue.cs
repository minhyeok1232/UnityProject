using UnityEngine;

public class RandomValue : MonoBehaviour
{
    public float value;
    public float increasedDamageValue;

    public void DamageRandom()
    {
        float multiplier = Random.Range(1.5f, 1.8f);
        increasedDamageValue = Mathf.RoundToInt(value * multiplier);
    }
}