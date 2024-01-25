using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public void Run()
    {
        gameObject.GetComponent<Animator>().SetBool("IsRunning", true);
    }    
    public void Idle()
    {
        gameObject.GetComponent<Animator>().SetBool("IsRunning", false);
    }    
    public void Die()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Dead");
    }

}
