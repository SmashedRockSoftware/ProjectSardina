using UnityEngine;
using System.Collections;
using System;

public class Gas : IComparable{
	public float gasAmount;
	public string gasName;

	public Gas(float amount, string name){
		gasAmount = amount;
		gasName = name;
	}

	public int CompareTo (object g1){
		if(g1 != null){
			Gas g = g1 as Gas;
			if(g != null){
				return -gasAmount.CompareTo(g.gasAmount);
			}else{
				throw new ArgumentException("Object is not a Gas"); 
			}
		}else{
			return 1;
		}
	}

	public override string ToString(){
		return gasName + ": " + gasAmount;
	}
}
