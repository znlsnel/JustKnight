using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
	// Start is called before the first frame update

	protected void FinishQestStep()
	{

	}
}

// ����Ʈ �������
// ���� ����, �۾�(Task), ����, ������

// Count Task
// Target Category, Count -> ���ۼ� ��ȭ 3��, ���� Ž�� 3��, ���� ��ȯ 5��, ����Ʈ���� Ȯ�� 1��, NPC�� ��ȭ�ϱ� 1��

// Taks �������
// ī�װ� - üũ�� string, �̸�, Action

// ����Ʈ ������ 
// ī�װ�, Ÿ��(����)�� �����ֵ� ã�Ƽ� ī����

// ���Ͱ� ������ Action
// -> ����Ʈ �����Ϳ� ����


// �ᱹ ��� ����Ʈ�� ���� ���� ����
// ����Ʈ�� ������ ����Ǵ� �κп��� (���Ͱ� �װų� �ϴ� �κ�) ����Ʈ ����Ʈ�� ����
// ����Ʈ ����Ʈ�� ���Ϳ��� ����
// ����Ʈ  ����Ʈ���� ī�װ��� Ÿ���� �����Ǿ�����
// �ش� ī�װ��� Ÿ���� ���� ����Ʈ�� ã�Ƽ� ī��Ʈ�� ++����


