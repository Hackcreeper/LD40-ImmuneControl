using System;
using UnityEngine;

namespace LD40
{
    public class EntityHealth : MonoBehaviour
    {
        public int Health;

        public EventHandler<EventArgs> OnDead;

        public bool FakeDeath = false;
        
        public bool Sub(int amount)
        {
            Health -= amount;

            if (Health > 0) return false;
            
            if (OnDead != null)
            {
                OnDead.Invoke(this, EventArgs.Empty);
            }

            if (!FakeDeath)
            {
                Destroy(gameObject);
            }

            return true;
        }
    }
}