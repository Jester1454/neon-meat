﻿using System.Collections.Generic;
using PlayerControllers;
using UnityEngine;

namespace UI
{
	public class CurrentAbilityView : MonoBehaviour
	{
		[SerializeField] private GameObject _bigBoyCard;
		[SerializeField] private GameObject _currentBigBoyCard;
		[SerializeField] private GameObject _smolCard;
		[SerializeField] private GameObject _currentSmolCard;
		[SerializeField] private GameObject _doubleJumpCard;
		[SerializeField] private GameObject _doubleJumpBigCard;
		[SerializeField] private GameObject _dashCard;
		[SerializeField] private GameObject _dashBigCard;		
		[SerializeField] private GameObject _laserCard;
		[SerializeField] private GameObject _laserBigCard;
		[SerializeField] private Transform _currentCardParent;
		[SerializeField] private Transform _otherCardParent;
		
		private AbilityController _player;
		private GameObject _currentCardObject;
		private List<GameObject> _otherCardsCollections = new List<GameObject>();
	
		private void Awake()
		{
			_player = FindObjectOfType<AbilityController>();
			_player.OnAbilityChanges += OnAbilityChanges;
		}

		private void OnAbilityChanges()
		{
			if (_currentCardObject != null)
			{
				Destroy(_currentCardObject);
			}

			foreach (var card in _otherCardsCollections)
			{
				Destroy(card);
			}
			_otherCardsCollections.Clear();

			if (_player.CurrentAbilities.Count == 0) return;
			
			
			var cardsCollection = new Stack<AbilityType>(new Stack<AbilityType>(_player.CurrentAbilities));
			var currentCardType = cardsCollection.Pop();

			switch (currentCardType)
			{
				case AbilityType.Smol:
					_currentCardObject = Instantiate(_currentSmolCard, _currentCardParent);
					break;
				case AbilityType.BigBoy:
					_currentCardObject = Instantiate(_currentBigBoyCard, _currentCardParent);
					break;
				case AbilityType.DoubleJump:
					_currentCardObject = Instantiate(_doubleJumpBigCard, _currentCardParent);
				break;				
				case AbilityType.Dash:
					_currentCardObject = Instantiate(_dashBigCard, _currentCardParent);	
					break;
				case AbilityType.Laser:
					_currentCardObject = Instantiate(_laserBigCard, _currentCardParent);
				break;
			}

			foreach (var abilityType in cardsCollection)
			{
				switch (abilityType)
				{
					case AbilityType.Smol:
						_otherCardsCollections.Add(Instantiate(_smolCard, _otherCardParent));
						break;
					case AbilityType.BigBoy:
						_otherCardsCollections.Add(Instantiate(_bigBoyCard, _otherCardParent));
						break;					
					case AbilityType.DoubleJump:
						_otherCardsCollections.Add(Instantiate(_doubleJumpCard, _otherCardParent));
						break;					
					case AbilityType.Dash:
						_otherCardsCollections.Add(Instantiate(_dashCard, _otherCardParent));	
						break;
					case AbilityType.Laser:
						_otherCardsCollections.Add(Instantiate(_laserCard, _otherCardParent));
						break;
				}
			}
		}
	}
}