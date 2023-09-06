using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private GameObject _hitEffect;
	[SerializeField] private float _hitOffset = 0;
	[SerializeField] private float _maxLength;
	[SerializeField] private float _mainTextureLength = 1f;
	[SerializeField] private float _noiseTextureLength = 1f;
	[SerializeField] private List<ParticleSystem> _laserEffects;
	[SerializeField] private List<ParticleSystem> _hitEffects;
	[SerializeField] private float _collisionRadius = 0.5f;
	[SerializeField] private float _hitDuration;

	private Vector4 _length = new Vector4(1, 1, 1, 1);
	private bool _laserSaver = false;
	private bool _updateSaver = false;
	private LineRenderer _lineRenderer;
	private readonly Collider2D[] _hits = new Collider2D[50];
	private float _currentHitDuration;
	
	private static readonly int _mainTex = Shader.PropertyToID("_MainTex");
	private static readonly int _noise = Shader.PropertyToID("_Noise");
	
	private void Start()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		_lineRenderer.material.SetTextureScale(_mainTex, new Vector2(_length[0], _length[1]));
		_lineRenderer.material.SetTextureScale(_noise, new Vector2(_length[2], _length[3]));

		if (_lineRenderer == null || _updateSaver != false) return;

		_lineRenderer.SetPosition(0, transform.position);
		var hit = Physics2D.Raycast(transform.position, transform.forward, _maxLength);
		if (hit.collider != null)
		{
			_lineRenderer.SetPosition(1, hit.point);
			_hitEffect.transform.position = hit.point + hit.normal * _hitOffset;
			foreach (var effect in _laserEffects)
			{
				if (!effect.isPlaying) effect.Play();
			}

			_length[0] = _mainTextureLength * (Vector3.Distance(transform.position, hit.point));
			_length[2] = _noiseTextureLength * (Vector3.Distance(transform.position, hit.point));

			var size = Physics2D.OverlapCircleNonAlloc(hit.point, _collisionRadius, _hits);
			var hasHit = false;

			if (size > 0)
			{
				foreach (var circleHit in _hits)
				{
					if (circleHit == null)
						continue;
					
					var destroyableObject = circleHit.GetComponent<DestroyableObject>();
					if (destroyableObject != null)
					{
						hasHit = true;
						destroyableObject.Destroy();
					}
				}
			}

			if (hasHit)
			{
				_currentHitDuration = _hitDuration;
				SetActiveHit(true);
			}
			else
			{
				_currentHitDuration -= Time.deltaTime;

				if (_currentHitDuration < 0)
				{
					SetActiveHit(false);
				}
			}
		}
		else
		{
			var endPos = transform.position + transform.forward * _maxLength;
			_lineRenderer.SetPosition(1, endPos);
			_hitEffect.transform.position = endPos;
			SetActiveHit(false);

			_length[0] = _mainTextureLength * (Vector3.Distance(transform.position, endPos));
			_length[2] = _noiseTextureLength * (Vector3.Distance(transform.position, endPos));
		}

		if (_lineRenderer.enabled == false && _laserSaver == false)
		{
			_laserSaver = true;
			_lineRenderer.enabled = true;
		}
	}

	private void SetActiveHit(bool isActive)
	{
		foreach (var effect in _hitEffects)
		{
			if (!isActive)
			{
				if (effect.isPlaying) effect.Stop();
			}
			else
			{
				if (!effect.isPlaying) effect.Play();
			}
		}
	}

	public void DisablePrepare()
	{
		if (_lineRenderer != null)
		{
			_lineRenderer.enabled = false;
		}
		_updateSaver = true;
	
		foreach (var effect in _laserEffects)
		{
			if (effect.isPlaying) effect.Stop();
		}

		SetActiveHit(false);
	}
}