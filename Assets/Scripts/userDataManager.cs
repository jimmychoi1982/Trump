using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class userDataManager{

	public static string userID;
	public static string userPW;
	public static string userName;
	public static int time;

	public enum LEVEL{
		EASY,
		NORMAL,
		HARD
	}

	public static LEVEL level { get; set;}
}
