using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PerfabsController;



namespace interfaceApplication
{
    //Scene
    public interface SceneController
    {
        void loadResources();
    }

    //User Action
    public interface UserAction
    {
        void moveBoat();
        void PAndVIsClicked(PAndVController POrVCtrl);
        void restart();
    
    }


    //Perfabs Action
    public enum SSActionEventType : int { started, competeted }
    public interface ISSActionCallback
    {
        void SSActionEvent(
            SSAction source, SSActionEventType events = SSActionEventType.competeted,
            int intParam = 0, string strParam = null, Object ObjectParam = null
        );
    }


}




