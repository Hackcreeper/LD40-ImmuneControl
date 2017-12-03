using UnityEngine;

namespace LD40.Towers
{
    public class Macrophage : Tower
    {
        public LayerMask EnemyMask;
        
        private bool active;
        private Transform targetEnemy;
        private float timer = 2f;
        private bool killed;
        
        protected override void OnUpdate()
        {
            if (!active)
            {
                if (!CheckEnemy()) return;

                active = true;
                var enemy = GetNearestEnemy();

                var component = enemy.GetComponent<Enemy>();
                if (component.IsSticky() || component.IsPulled())
                {
                    return;
                }

                component.Pull(this);
                targetEnemy = enemy;
            }
            else
            {
                if (targetEnemy == null)
                {
                    if (Vector3.Distance(transform.localScale, Vector3.one) >= 0.02)
                    {
                        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 8 * Time.deltaTime);
                        return;
                    }
                    
                    active = false;
                    return;
                }

                var scale = killed ? Vector3.one : new Vector3(1.3f, 1.3f, 1.3f);
                var speed = killed ? 8 : 4;
                
                transform.localScale = Vector3.Lerp(
                    transform.localScale,
                    scale,
                    speed * Time.deltaTime
                );
                
                if (Vector3.Distance(transform.position, targetEnemy.position) > 0) return;

                if (targetEnemy.GetComponent<RotationAnimation>())
                {
                    Destroy(targetEnemy.GetComponent<RotationAnimation>());
                }
                
                timer -= Time.deltaTime;
                if (timer > 0) return;

                if (!killed)
                {
                    killed = true;
                    timer = .2f;
                }
                else
                {
                    targetEnemy.GetComponent<EntityHealth>().Sub(Damage);
                    killed = false;
                    timer = 2f;
                }
            }
        }

        private bool CheckEnemy()
        {
            return Physics.CheckSphere(transform.position, Radius, EnemyMask);
        }

        private Transform GetNearestEnemy()
        {
            return Physics.OverlapSphere(transform.position, Radius, EnemyMask)[0].transform;
        }

        // Falls aktiv:
            // Warten auf Kollision mit Gegner
            // Sollte Gegner währenddessen 'sticky' werden -> inaktiv
            // Sollte Gegner währenddessen sterben -> inaktiv
            // Wenn Gegner kollidiert, diesen in sich aufnehmen
            // Töten >~< und dabei eine kleine Animation abspielen^^
    }
}