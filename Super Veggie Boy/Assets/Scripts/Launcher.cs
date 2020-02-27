using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Transform launcherTransform = default;
    [SerializeField] private GameObject prefab = default;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPrefab), 0f, cooldown);
    }

    private void SpawnPrefab()
    {
        GameObject go = Instantiate(prefab, launcherTransform.position, launcherTransform.rotation);
        go.transform.localScale = launcherTransform.localScale;
        go.GetComponent<Rigidbody>().velocity = launcherTransform.forward * speed;
        Destroy(go, lifeTime);
    }
}
