using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void Attack();
    int GetScore();

    bool GetGameover();

    void Restart();

}
