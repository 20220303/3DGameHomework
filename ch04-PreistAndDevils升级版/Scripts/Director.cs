using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using PerfabsController;
using interfaceApplication;

public class Director : System.Object
{
    //µ¼ÑÝ£¬¿ØÖÆ³¡¾°ÇÐ»»
    private static Director _instance;
    public SceneController currentSceneController { get; set; }

    public static Director getInstance()
    {
        if (_instance == null)
        {
            _instance = new Director();
        }
        return _instance;
    }
}


    //public interface SceneController{
    //    void loadResources();
    //}


    //public interface UserAction
    //{
    //    void moveBoat();
    //    void PAndVIsClicked(PAndVController POrVCtrl);
    //    void restart();
    //    void boatShake();
    //}





