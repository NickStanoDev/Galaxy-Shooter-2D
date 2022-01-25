using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField]
    private int _resistance = 3;
    private int _maxResistance;
    private SpriteRenderer _shieldRenderer;
    private Color _normal, _damage1, _damage2;

    

    void Start()
    {
        _shieldRenderer = GetComponent<SpriteRenderer>();
        
        _maxResistance = _resistance;
        _normal = _shieldRenderer.color;
        _damage1 = new Color32(252, 196, 252, 193);
        _damage2 = new Color32(231, 45, 126, 197);

        if (_shieldRenderer == null)
        {
            Debug.LogError("Shield renderer is NULL");
        }

    }


    void Update()
    {

    }

    void SetShieldColor()
    {
        switch (_resistance)
        {

            case 0:
                transform.GetComponentInParent<Player>().ShutDownShield();
                break;
            
            case 1:
                _shieldRenderer.color = _damage2;
                break;
            case 2:
                _shieldRenderer.color = _damage1;
                break;
            case 3:
                _shieldRenderer.color = _normal;
                break;





        }


    }

    public void DamageShield()
    {
        _resistance--;
        SetShieldColor();

 
    }

    public void RestoreShield()
    {
        _resistance = _maxResistance;
        SetShieldColor();
    }

}
