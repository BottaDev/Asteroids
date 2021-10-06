using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P2
public interface ICommand
{
    void Execute(params object[] parameters);
}
