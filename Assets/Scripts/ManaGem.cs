using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGem : MonoBehaviour
{
    public GameObject player;
    public float lifeTime = 1f;
    
    private float _timeLeft;
    private bool _active = false;


    private void Update()
    {
        if (_active)
        {
            if (_timeLeft < Time.time)
            {
                player.GetComponent<ManaManager>().SetHasGem(false, null, false);
            }
        }

    }

    public void setActive()
    {
        _active = true;
        _timeLeft = Time.time + lifeTime;
    }

}
