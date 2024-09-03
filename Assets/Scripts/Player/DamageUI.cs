using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
	[SerializeField] Text _damageText;
	[SerializeField] float _damageDisplayTime;

	[NonSerialized] public int _damage;

	Animator _anim;
	public GameObject _parent;
	private void Awake()
	{
		_anim = GetComponent<Animator>();
		
	}
	private void Update()
	{
		
		transform.localPosition = _parent.transform.position;
	}


	public void SetDamage(int damage)
	{
		if (damage == 0)
			return;

		if (!gameObject.activeSelf)
			gameObject.SetActive(true);

		_anim.Play("DamageUI", 0, 0.0f);
		//_anim.play

		_damage += damage;
		_damageText.text = _damage.ToString();
	}

	public void AE_EndDamageUI()
	{
		gameObject.SetActive(false);
		_damage = 0;
		_damageText.text = "";
	}
}
