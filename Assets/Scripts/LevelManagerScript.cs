using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour {
    
    public int scrollSpeed = 20;
    private bool canMove = false;

    void Update() {
        if(this.canMove) Move();
    }

    private void Move() {
        this.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }

    public void StartMove() {
        this.canMove = true;
    }

    public void StopMove() {
        this.canMove = false;
    }

    public void Restart() {
        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
    }
}
