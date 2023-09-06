using Input;
using PlayerControllers;
using UnityEngine;

public class CommonDirectionAbility : DirectionAbility
{
	private readonly NewInputDriver _driver;

	public CommonDirectionAbility(Transform playerTransform, DirectionView directionView, float directionRadius, NewInputDriver driver) : base(playerTransform, directionView, directionRadius)
	{
		_driver = driver;
	}
	
	protected override void UpdateDirection(Vector2 lookPosition, Transform direction, float radius)
	{
		if (_driver.PlayerInput.user.controlScheme?.name == _driver.PlayerControls.KeyboardMouseScheme.name)
		{
			var newLocation = (Vector2) _playerTransform.position + lookPosition;
			var centerPosition = (Vector2) _playerTransform.localPosition; 
			//var distance = Vector2.Distance(newLocation, centerPosition);

			//if (distance > radius)
			{
				var fromOriginToObject = newLocation - centerPosition;
				//fromOriginToObject *= radius / distance;
				newLocation = centerPosition + fromOriginToObject;
			}

			newLocation = Camera.main.WorldToViewportPoint(newLocation);
			newLocation.x = Mathf.Clamp01(newLocation.x);
			newLocation.y = Mathf.Clamp01(newLocation.y);
			newLocation = Camera.main.ViewportToWorldPoint(newLocation);
			
			direction.position = newLocation;
		}
		else
		{
			base.UpdateDirection(lookPosition, direction, radius);
		}
		
		if (!(lookPosition.magnitude > 0.001f)) return;

		if (!direction.gameObject.activeSelf)
		{
			direction.gameObject.SetActive(true);
		}
	}
}