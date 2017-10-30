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
			Debug.Log(NameBuilder.RandomFullName(Gender.FEMALE, "EarlyRoman"));
			Debug.Log(NameBuilder.RandomFullName(Gender.MALE, "Irish"));
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
