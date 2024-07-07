using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "increase", menuName = "Quest/TaskAction/increase", order = 1)]

public class TaskActionSO_InCrease : TaskActionSO
{
	public override int Run(int curCnt, int addPoint)
	{
		return curCnt + addPoint;
	}
}
