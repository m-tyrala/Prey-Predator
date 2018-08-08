using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlls
{
	public Hashtable get;

	public Controlls(string PlayerName)
	{
		switch (PlayerName)
		{
				case "Predator1":
					get = new Hashtable();
					get.Add("thrust", KeyCode.W);
					get.Add("nitro", KeyCode.X);
					get.Add("left", KeyCode.A);
					get.Add("right", KeyCode.D);
					get.Add("howl", KeyCode.Q);
					get.Add("purr", KeyCode.E);
					break;
				case "Predator2":
					get = new Hashtable();
					get.Add("thrust", KeyCode.T);
					get.Add("nitro", KeyCode.B);
					get.Add("left", KeyCode.F);
					get.Add("right", KeyCode.H);
					get.Add("howl", KeyCode.R);
					get.Add("purr", KeyCode.Y);
					break;
				case "Predator3":
					get = new Hashtable();
					get.Add("thrust", KeyCode.I);
					get.Add("nitro", KeyCode.Comma);
					get.Add("left", KeyCode.J);
					get.Add("right", KeyCode.L);
					get.Add("howl", KeyCode.U);
					get.Add("purr", KeyCode.O);
					break;	
				case "Predator4":
					get = new Hashtable();
					get.Add("thrust", KeyCode.UpArrow);
					get.Add("nitro", KeyCode.DownArrow);
					get.Add("left", KeyCode.LeftArrow);
					get.Add("right", KeyCode.RightArrow);
					get.Add("howl", KeyCode.RightShift);
					get.Add("purr", KeyCode.RightControl);
					break;
		} 
	} 
}
