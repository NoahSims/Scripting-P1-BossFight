using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;
    public int CurrentHealth { get => _currentHealth; }
    [SerializeField] private bool _isInvincible;
    public bool IsInvincible
    {
        get => _isInvincible;
        set => _isInvincible = value;
    }

    [SerializeField] ParticleSystem _damageParticles;
    [SerializeField] AudioClip _damageSound;
    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] AudioClip _deathSound;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!_isInvincible)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Kill();
            }
            else
            {
                Feedback(_damageParticles, _damageSound);
            }
        }
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth += amount;
        Mathf.Clamp(_currentHealth, 0, _maxHealth);
    }

    public void Kill()
    {
        Feedback(_deathParticles, _deathSound);
        gameObject.SetActive(false);
    }

    private void Feedback(ParticleSystem particles, AudioClip sound)
    {
        // particles
        if (particles != null)
        {
            ParticleSystem _particles = Instantiate(particles, transform.position, Quaternion.identity);
            _particles.Play();
        }
        // audio. TODO - consider Object Pooling for performance
        if (sound != null)
        {
            AudioHelper.PlayClip2D(sound, 1f);
        }
    }
}
