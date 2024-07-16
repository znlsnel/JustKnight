using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "increase", menuName = "Quest/TaskAction/increaseDeltaTime", order = 1)]

public class TA_InCreaseDelta : TaskActionSO
{
	float time = 0.0f;
	public override int Run(int curCnt, int addPoint)
	{
		time += addPoint * Time.deltaTime;
		if (time > addPoint)
		{
			time = 0.0f;
			return curCnt + addPoint;
		}
		return curCnt;
	}
}
