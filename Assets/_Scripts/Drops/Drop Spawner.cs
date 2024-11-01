using UnityEngine;

[System.Serializable]
public class DropSpawner
{
    public virtual void SpawnClusterDrop(GameObject dropper, int dropCount, GameObject dropToSpawn = null)
    {
        int dropCountToUse;
        if (dropCount == 0)
            dropCountToUse = Random.Range(1, 5);
        else
            dropCountToUse = dropCount;

        for (int i = 0; i < dropCountToUse; i++)
            SpawnDrop(dropper, dropToSpawn);
    }

    public virtual void SpawnDrop(GameObject dropper, GameObject dropToSpawn = null)
    {
        GameObject prefabToUse;
        if (dropToSpawn != null)
            prefabToUse = dropToSpawn;
        else
            prefabToUse = PlayerConfigs.Instance.dropPrefab[Random.Range(0, PlayerConfigs.Instance.dropPrefab.Length)];

        var defaultSpawn = dropper.transform.position + Vector3.up * .5f;

        GameObject drop = Object.Instantiate(prefabToUse, defaultSpawn, Quaternion.identity);

        drop.transform.LookAt(GetRandomPointInRadius(PlayerConfigs.Instance.dropRadius), Vector3.up);
        drop.GetComponent<Rigidbody>().AddForce(drop.transform.forward * 2.5f, ForceMode.Impulse);

        Vector3 GetRandomPointInRadius(float radius)
        {
            Vector3 pointAboveTerrain;
            RaycastHit hit;
            do
            {
                float randomX = Random.Range(-radius, radius);
                float randomZ = Random.Range(-radius, radius);

                pointAboveTerrain = prefabToUse.transform.position + new Vector3(randomX, 100, randomZ);

            } while (Physics.Raycast(pointAboveTerrain, Vector3.down, out hit, 200f, LayerMask.GetMask("Terrain")) == false);

            Vector3 targetPoint = hit.point;
            targetPoint.y += 4f;   // offset from ground

            return targetPoint;
        }
    }
}