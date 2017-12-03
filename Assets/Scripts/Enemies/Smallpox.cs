using UnityEngine;

namespace LD40.Enemies
{
    public class Smallpox : Enemy
    {
        private bool infestedSomeone;
        
        protected override void OnSticky(Tower tower)
        {
            if (infestedSomeone) return;

            var position = tower.transform.position;
            var rotation = tower.transform.rotation;
            
            tower.GetComponent<EntityHealth>().Sub(tower.GetComponent<EntityHealth>().Health);

            var infested = Instantiate(EnemySpawner.Instance.InfestedAntibodyPrefab);
            infested.transform.position = position;
            infested.transform.rotation = rotation;
            infested.GetComponent<Enemy>().SetNode(currentNode);
            
            EnemySpawner.Instance.IncreaseEnemiesLeft();
            
            infestedSomeone = true;
        }

        protected override void OnUpdate()
        {
            if (infestedSomeone) return;

            if (pulledBy != null)
            {
                if (Vector3.Distance(transform.position, pulledBy.position) <= 0.34f)
                {
                    var position = pulledBy.transform.position;
                    var rotation = pulledBy.transform.rotation;
                    
                    pulledBy.GetComponent<EntityHealth>().Sub(pulledBy.GetComponent<EntityHealth>().Health);
                    
                    var infested = Instantiate(EnemySpawner.Instance.InfestedMacrophagePrefab);
                    infested.transform.position = position;
                    infested.transform.rotation = rotation;
                    infested.GetComponent<Enemy>().SetNode(currentNode);
            
                    EnemySpawner.Instance.IncreaseEnemiesLeft();
            
                    infestedSomeone = true;
                }
            }
        }
    }
}