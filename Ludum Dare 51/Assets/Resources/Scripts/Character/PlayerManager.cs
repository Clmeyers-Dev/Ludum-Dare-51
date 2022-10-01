using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] heartSprites = new GameObject[5];
    [SerializeField]
    private int hearts;
    [SerializeField]
    private int maxHearts;
    [SerializeField]
    private int currentHearts;
    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void loseHealth(){
        for(int i = 0; i < heartSprites.Length;i++){
            if(heartSprites[i].activeSelf){
                heartSprites[i].SetActive(false);
                return;
            }
        }
    }
  public void gainHealth(){
        for(int i = heartSprites.Length-1;i>=0;i--){
            if(!heartSprites[i].activeSelf){
                heartSprites[i].SetActive(true);
                return;
            }
        }
    }
    public int getNumberOfHealth(){
        int count=0;
       for(int i = 0; i < heartSprites.Length;i++){
           if(heartSprites[i].activeSelf){
               count++;
           }
       }

        return count;
    }
}
