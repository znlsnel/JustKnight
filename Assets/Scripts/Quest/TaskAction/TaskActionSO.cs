using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TaskActionSO : ScriptableObject
{
    // Start is called before the first frame update
    public int successCount;

	public abstract int Run(int curCnt, int addPoint );
}
