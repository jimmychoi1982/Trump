using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class userDataManager{

	public static string userID;
	public static string userPW;
	public static string userName;
	public static int IconIndex;
	public static string greet;

	public static int winTime;
	public static int loseTime;
	public static int percentage = 0; // 勝率

	public static int easyClearTime = -1; // millisecondsで記録
	public static int normalClearTime = -1;
	public static int hardClearTime = -1;

	public static MULTI_STATE multiState = MULTI_STATE.None;

	public enum MULTI_STATE{
		None,
		Master,
		Guest
	}

	public enum LEVEL{
		EASY,
		NORMAL,
		HARD
	}

	public static LEVEL level { get; set;}
}
