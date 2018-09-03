﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private Platform platformPrefab;
	[SerializeField]
	private Platform movingPlatformPrefab;
	[SerializeField]
	private Platform disappearingPlatform;
	[SerializeField]
	private GameObject cookiePrefab;
	[SerializeField]
	private Transform player;

	private float width = 5f; // ширина появления платформ
	private float minY = .5f, maxY = 1.5f; // расстояния до следующей платформы
	private int numberOfPlatforms = 200;
	private int numberOfCookie = 100;
	private List<Vector3> platformPosition = new List<Vector3>(); // храним позиции платформ в отдельном списке
	private List<Platform> platforms = new List<Platform>();

	// Use this for initialization
	void Start () {
		Vector3 spawnPosition = new Vector3();

		// генерируем позиции платформ в начале уровня
		for (int i = 0; i < numberOfPlatforms; i++) {
			spawnPosition.y += Random.Range(minY, maxY);
			spawnPosition.x = Random.Range(-width, width);
			platformPosition.Add(spawnPosition);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// удаляем платформы, которые ниже камеры
		if (platforms.Count != 0) {
			foreach (Platform platform in platforms) {
				if (platform.transform.position.y < player.position.y - 7f) {
					platforms.Remove(platform);
					Destroy(platform.gameObject);
					break;
				}
			}
		}
		// добавляем платформы, положения которых приближаются к игроку
		if (platformPosition.Count != 0) {
			foreach (Vector3 position in platformPosition) {
				if (position.y < player.transform.position.y + 7f) {
					Platform platform = platformPrefab;
					int random = Random.Range(0, 9);
					// иногда появляются двигающиеся платформы
					if (random == 5) {
						platform = movingPlatformPrefab;
					}
					// и исчезающие
					else if (random == 1) {
						platform = disappearingPlatform;
					}
					if (Random.Range(0, 9) == 2 && platform != movingPlatformPrefab && numberOfCookie != 0) {
						Vector3 cookiePosition = position;
						GameObject cookie = Instantiate(cookiePrefab, position, Quaternion.identity);

						cookiePosition.y += platform.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2 +
							cookie.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2;
						cookie.transform.position = cookiePosition;
						numberOfCookie--;
					}
					platforms.Add(Instantiate(platform, position, Quaternion.identity));
					if (random == 0) {
						platforms[platforms.Count - 1].JumpForce = 50f;
						platforms[platforms.Count - 1].SetColor(Color.red);
					}
					platformPosition.Remove(position);
					break;
				}
			}
		}
	}
}
