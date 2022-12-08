using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//固定代码
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, int intParam = 0, GameObject objectParam = null);
}
