using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReminder 
{
    void MakeSnapshot();
    void Rewind();

    IEnumerator StartToRecord();
}
