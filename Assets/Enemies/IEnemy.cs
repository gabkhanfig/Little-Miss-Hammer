using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
        Health GetHealthComponent();
        void OnDamageEvent(int damageAmount, Vector2 damageDirection, bool isDead);
        void OnHealingEvent(int healingAmount);
}
