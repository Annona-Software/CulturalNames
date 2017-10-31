using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CulturalNames;

public class Test : MonoBehaviour
{
	// Use this for initialization
	void Start () 
	{
		NameBuilder.LoadNames();
		for(int i = 0; i < 10; i++)
		{
			Debug.Log(NameBuilder.RandomUniqueName(Gender.FEMALE, "EarlyRoman").ToString());
			Debug.Log(NameBuilder.RandomUniqueName(Gender.MALE, "Irish").ToString());
		}
			Debug.Log(NameBuilder.RandomName(Gender.FEMALE, "wrong name")); //this should err

	}
}
