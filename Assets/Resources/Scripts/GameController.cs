﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField]
	private GameObject playerObj;
	[SerializeField]
	private GameObject camera;
	[SerializeField]
	private GameObject loseWidget;
	[SerializeField]
	private GameObject darkScreen;
	[SerializeField]
	private ScoreBar scoreBar;
	private GameData gameData;

	private float deltaDark = 2f;
	private bool lose = false;
	private bool saved = false; // чтобы игра постоянно не сохранялась при проигрыше

	private SpriteRenderer darkScreenSprite;
	private Camera gameCamera;
	private Player player;

	private float gameTime = 0f; // время в игре

	// Use this for initialization
	void Start () {
		darkScreenSprite = darkScreen.GetComponent<SpriteRenderer>();
		gameData = GameObject.Find("GameData").GetComponent<GameData>();
		gameCamera = camera.GetComponent<Camera>();
		player = playerObj.GetComponent<Player>();
		gameData.SetValue("lastJumps", 0);
		gameData.SetValue("lastCookies", 0);
		gameData.SetValue("lastPlayTime", 0);
		gameData.IncrementValue("playedGames", 1);
		gameData.SaveData(); // сохраняем игру при старте
	}

	void CheckMax(string name, int value) {
		if (value > gameData.GetXmlValue(name)) {
			gameData.SetValue(name, value);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// рассеивание черного экрана перед стартом игры
		if (darkScreenSprite.color.a > 0 && !loseWidget.activeSelf) {
			Color color = darkScreenSprite.color;
			color.a -= deltaDark * Time.deltaTime;
			darkScreenSprite.color = color;
		}

		// если игрок упал
		if (playerObj.transform.position.y < camera.transform.position.y - 7f) {
			lose = true;
			gameCamera.Down = true;
			player.CanMove = false;
		}

		if (!lose) {
			gameTime += Time.deltaTime;
		}

		// когда камера перестала падать за игроком
		if (gameCamera.Standing) { 
			loseWidget.SetActive(true);
			if (!saved) {
				// очки
				gameData.SetValue("lastScore", scoreBar.Score);
				CheckMax("maxScore", scoreBar.Score);
				gameData.IncrementValue("totalScore", scoreBar.Score);

				// прыжки
				CheckMax("maxJumps", gameData.GetValue("lastJumps"));
				gameData.IncrementValue("totalJumps", gameData.GetValue("lastJumps"));

				// печеньки
				CheckMax("maxCookies", gameData.GetValue("lastCookies"));
				gameData.IncrementValue("totalCookies", gameData.GetValue("lastCookies"));

				// время
				gameData.SetValue("lastPlayTime", (int)gameTime);
				CheckMax("longestPlayTime", gameData.GetValue("lastPlayTime"));
				gameData.IncrementValue("totalPlayTime", gameData.GetValue("lastPlayTime"));

				gameData.SaveData();
				saved = true;
			}
		}
	}
}
