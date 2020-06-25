using UnityEngine.Networking;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    
    [SyncVar]
    private int currentHealth;

    private void Awake()
    {
        SetDefaults();
    }

    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + "now has" + currentHealth + "health");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
