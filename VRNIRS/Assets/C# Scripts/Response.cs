using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta;
using Meta.HandInput;
using UnityEngine.SceneManagement;



public class Response : Interaction
{

    public GameObject _boutton;
    


    private HandFeature _handFeature; //follow the hand during a grab
    private GameObject _selectedGameObject;
      
    protected override void Engage() //when it's in the zone, hand close
    {
        _handFeature = GrabbingHands[0];

        if (_handFeature == null)

        {
            _selectedGameObject = Instantiate(_boutton); //create a clone to move  object
            
        }

        Debug.Log("grabbed");
        //appeler la fonction du on clic
        


    }
    protected override void Disengage()
    {
        if (_handFeature == null || _selectedGameObject == null)

        {

            return;
        }

        _selectedGameObject.SendMessage("detach");
        _selectedGameObject = null;
        
        
        

    }

    protected override void Manipulate()
    {
        return;
    }
}
