using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonist : MonoBehaviour {

    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private CharacterController _controller;

	public void Poll()
    {
        InputData input = new InputData()
        {
            MoveDown = Input.GetKey(KeyManager.MoveDown),
            MoveLeft = Input.GetKey(KeyManager.MoveLeft),
            MoveRight = Input.GetKey(KeyManager.MoveRight),
            MoveUp = Input.GetKey(KeyManager.MoveUp),
        };

        Process(input);
    }
    private void Process(InputData input)
    {
        Vector2 moveDirection = new Vector2();

        if (input.MoveDown)
            moveDirection.y -= 1;

        if (input.MoveUp)
            moveDirection.y += 1;

        if (input.MoveLeft)
            moveDirection.x -= 1;

        if (input.MoveRight)
            moveDirection.x += 1;

        Move(moveDirection);
    }
    private void Move(Vector2 moveDirection)
    {
        Vector2 delta = moveDirection * (_movementSpeed * Time.deltaTime);

        _controller.Move(delta);
    }
    private struct InputData
    {
        public bool MoveLeft;
        public bool MoveRight;
        public bool MoveUp;
        public bool MoveDown;
    }
}
