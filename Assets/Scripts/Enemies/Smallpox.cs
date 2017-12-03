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
    }
}