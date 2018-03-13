using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _chunkPollInterval = 1;

    private float _timeSinceLastPoll;

    private void Awake()
    {
        _timeSinceLastPoll = _chunkPollInterval;
    }
    private void Update()
    {
        Move();
        PollMapGenerator();
    }
    private void Move()
    {
        transform.position = Vector2.Lerp(transform.position, ColonistManager.SelectedColonist.transform.position, _speed * Time.deltaTime);
    }
    private void PollMapGenerator()
    {
        if(_timeSinceLastPoll >= _chunkPollInterval)
        {
            _timeSinceLastPoll = 0;

            MapGenerator.Poll();
        }
        else
        {
            _timeSinceLastPoll += Time.unscaledDeltaTime;
        }
    }
}
