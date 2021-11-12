using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta;
using Meta.HandInput;
using UnityEngine.SceneManagement;



public class CloneGrab : Interaction
{

    [SerializeField] GameObject _object; //what are we cloning
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] GameObject _boutton;


    private HandFeature _handFeature; //follow the hand during a grab
    private GameObject _heldGameObject;
    

    //private readonly GameObject button;



    protected override void Engage() //when it's in the zone, hand close
    {
        _handFeature = GrabbingHands[0];

        if (_handFeature == null)

        {
            _heldGameObject = Instantiate(_object); //create a clone to move  object
            




        }
        Debug.Log("grabbed");
        Input.GetButtonDown(_boutton.name);
        Debug.Log("Clicked on : " + _boutton.name);


    }
    protected override void Disengage()
   {
        if (_handFeature == null || _heldGameObject==null)

        {
            
            return;
        }
       
        _heldGameObject.SendMessage("detach");
        _heldGameObject = null;

         

            
            
    }

    protected override void Manipulate()
    {
        throw new System.NotImplementedException();
    }
}
