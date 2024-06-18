using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnPointY;
    [SerializeField] private float _minSpawnPointX;
    [SerializeField] private float _maxSpawnPointX;
    [SerializeField] private float _minSpawnPointZ;
    [SerializeField] private float _maxSpawnPointZ;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private Cube _cube;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: OnGet,
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    public void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);

        cube.LifeTimeEnded -= ReleaseCube;
    }

    private void OnGet(Cube cube)
    {
        cube.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        cube.Rigidbody.velocity = Vector3.zero;
        cube.LifeTimeEnded += ReleaseCube;
        cube.gameObject.SetActive(true);
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (true)
        {
            _pool.Get();

            yield return wait;
        }
    }
}