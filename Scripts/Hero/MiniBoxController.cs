using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoxController : MonoBehaviour
{
    public GameObject currentHero; // Hero đang đứng trong ô này
    public bool isHasHero = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Nếu hero đứng trong box
        if (other.CompareTag("Player"))
        {
            if (!isHasHero)
            {
                isHasHero = true;
                currentHero = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Nếu hero rời khỏi box
        if (other.CompareTag("Player"))
        {
            if (currentHero == other.gameObject)
            {
                isHasHero = false;
                currentHero = null;
            }
        }
    }
}