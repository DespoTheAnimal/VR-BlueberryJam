using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRrigReference : MonoBehaviour
{
    public static VRrigReference Singleton;

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    void Awake(){

        //Singleton pattern
        if(Singleton == null){
            Singleton = this;
        }else{
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
