using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TenSecondManager : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    public string[] states = new string[10];
    public string state;
    public bool paused;
    public Animator animator;
    void Rotate(){
        
    }
    private void Update() {
        if(!paused){
        animator.Play("Spin");
        }else{
            animator.Play("Idle");
        }
        stateText.text = state;
    }
    void changeState(){
        int random = Random.Range(0,1000);
        Debug.Log(random);
        if(random >=0 && random <= 100){
            state = states[1];
        }else if(random >100 && random <= 150){
            state = states[2];
        }else if(random >150 && random <= 225){
            state = states[3];
        }else if(random >225 && random <= 500){
            state = states[4];
        }else if(random >500 && random <= 1000){
            state = states[5];
        }
    }
}
