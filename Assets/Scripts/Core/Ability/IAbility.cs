using UnityEngine;

public interface IAbility
{
	public void Update(float deltaTime, Vector2 lookPosition);
	public void Cancel();
	public void Use(Vector2 lookPosition);
}