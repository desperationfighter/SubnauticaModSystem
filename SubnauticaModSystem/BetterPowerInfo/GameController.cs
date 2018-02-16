﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BetterPowerInfo
{
	public class GameController : MonoBehaviour
	{
		private PowerProductionDisplay productionDisplay;
		private PowerConsumerDisplay consumerDisplay;

		private void Awake()
		{
			DevConsole.RegisterConsoleCommand(this, "deplete", false, false);
			Logger.Log("GameController Added");
		}

		private void OnDestroy()
		{
			Logger.Log("GameController Destroyed");
		}

		private void Update()
		{
			if (!uGUI_SceneLoading.IsLoadingScreenFinished || uGUI.main == null || uGUI.main.loading.IsLoading)
			{
				return;
			}

			if (productionDisplay == null)
			{
				Logger.Log("Creating Text Objects...");
				Transform hud = GameObject.FindObjectOfType<uGUI_PowerIndicator>().transform;
				productionDisplay = CreateNewText(hud, -500, TextAnchor.UpperRight).AddComponent<PowerProductionDisplay>();
				consumerDisplay = CreateNewText(hud, 500, TextAnchor.UpperLeft).AddComponent<PowerConsumerDisplay>();
			}

			if (productionDisplay != null && consumerDisplay != null)
			{
				bool keyDown = Input.GetKey(KeyCode.P);
				if (keyDown && productionDisplay.Mode == DisplayMode.Minimal)
				{
					productionDisplay.SetMode(DisplayMode.Verbose);
					consumerDisplay.SetMode(DisplayMode.Verbose);
				}
				else if (!keyDown && productionDisplay.Mode == DisplayMode.Verbose)
				{
					productionDisplay.SetMode(DisplayMode.Minimal);
					consumerDisplay.SetMode(DisplayMode.Minimal);
				}
			}

			//if (Input.GetKeyDown(KeyCode.I)) MoveAllDisplays(new Vector2(0, 1));
			//if (Input.GetKeyDown(KeyCode.K)) MoveAllDisplays(new Vector2(0, -1));
			//if (Input.GetKeyDown(KeyCode.J)) MoveAllDisplays(new Vector2(-1, 0));
			//if (Input.GetKeyDown(KeyCode.L)) MoveAllDisplays(new Vector2(1, 0));
		}

		//private void MoveAllDisplays(Vector2 offset)
		//{
		//	productionDisplay.text.rectTransform.anchoredPosition += offset;
		//	consumerDisplay.text.rectTransform.anchoredPosition += new Vector2(-offset.x, offset.y);
		//	Logger.Log("Position = " + productionDisplay.text.rectTransform.anchoredPosition);
		//}

		private void OnConsoleCommand_deplete()
		{
			if (Player.main != null && Inventory.main != null)
			{
				foreach (InventoryItem inventoryItem in Inventory.main.container)
				{
					if (inventoryItem.item.GetTechType() == TechType.Battery ||
						inventoryItem.item.GetTechType() == TechType.PowerCell)
					{
						var battery = inventoryItem.item.GetComponent<Battery>();
						if (battery)
						{
							battery.charge = 0;
						}
					}
				}
			}
			ErrorMessage.AddDebug("Depleting batteries and powercells");
		}

		private static GameObject CreateNewText(Transform parent, int x, TextAnchor anchor)
		{
			Text prefab = GameObject.FindObjectOfType<HandReticle>().interactPrimaryText;
			if (prefab == null)
			{
				Logger.Log("Could not find text prefab! (HandReticle.interactPrimaryText)");
				return null;
			}

			Text text = GameObject.Instantiate(prefab);
			text.gameObject.layer = parent.gameObject.layer;
			text.gameObject.name = "PowerIndicatorDisplayText";
			text.transform.SetParent(parent, false);
			text.transform.localScale = new Vector3(1, 1, 1);
			text.gameObject.SetActive(true);
			text.enabled = true;
			text.text = "";
			text.fontSize = 20;
			RectTransformExtensions.SetParams(text.rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 0.5f), parent);
			text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 700);
			text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 800);
			text.rectTransform.anchoredPosition = new Vector3(x, -428);
			text.alignment = anchor;
			text.raycastTarget = false;

			return text.gameObject;
		}
	}
}
