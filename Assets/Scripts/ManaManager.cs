using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    public float currentMana;
    public float maxMana;
    public float regenSpeed;
    public Slider sliderObject;

    public float gemRegenValue = 5;

    private bool _hasAGem = false;
    private GameObject _equippedGem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AddMana(regenSpeed * Time.deltaTime);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Gem"))
        {            
            SetHasGem(true, collision.gameObject);
        }
    }


    public void AddMana(float value)
    {
        if (currentMana + value < maxMana)
        {
            if(currentMana + value > 0)
            {
                currentMana += value;
                sliderObject.value = currentMana / maxMana;
            }
            else
            {
                currentMana = 0;
                sliderObject.value = 0;
            }

        }
        else
        {
            currentMana = maxMana;
            sliderObject.value = 1f;

        }
    }

    public void SetHasGem(bool value, GameObject gemObject)
    {
        if (value)
        {
            if (!_hasAGem)
            {
                _hasAGem = true;
                _equippedGem = gemObject;

                gemObject.transform.SetParent(gameObject.transform);
                gemObject.transform.localPosition = new Vector2(0, 1f);
                return;
            }
        } else
        {
            if (_hasAGem)
            {
                _hasAGem = false;
                Destroy(_equippedGem);
                _equippedGem = null;
                AddMana(gemRegenValue);

                return;
            }
        }
    }
}
