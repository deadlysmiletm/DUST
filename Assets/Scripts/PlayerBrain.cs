using UnityEngine;

public class PlayerBrain : MonoBehaviour {

    private Character chara;
    private bool _tap;
    public bool withMouse;
    public bool withKeyboard;
    public bool death;

	void Awake ()
    {
        Time.timeScale = 1;
        chara = this.GetComponent<Character>();
	}
	
	void Update ()
    {
        if (chara.actualNode == null)
            return;

        else if (!chara.actualNode.finalNode)
        {
            if (withMouse)
            {
                GetMouseTouch();
            }
            else
            {
                GetTouch();
            }

            if (chara.move == false)
            {
                var touchDirection = (withMouse) ? GetMouseDirection() : (withKeyboard) ? GetKeyboardDirection() : GetDirection();

                if (touchDirection == Vector2.up && chara.actualNode.moveUp)
                {
                    chara.dir = 1;
                    chara.move = true;
                    chara.nodeCount++;
                }
                else if (touchDirection == Vector2.down && chara.actualNode.moveDown)
                {
                    chara.dir = 2;
                    chara.move = true;
                    chara.nodeCount++;
                }
                else if (touchDirection == Vector2.right && chara.actualNode.moveRight)
                {
                    chara.dir = 3;
                    chara.move = true;
                    chara.nodeCount++;
                }
                else if (touchDirection == Vector2.left && chara.actualNode.moveLeft)
                {
                    chara.dir = 4;
                    chara.move = true;
                    chara.nodeCount++;
                }
            }

            chara.Move();

            if(death)
            {
                chara.Death();
            }
        }
        else
        {
            chara.LevelEnded();
            StartCoroutine(DeactiveAfterTime(5));
        }
    }

    private System.Collections.IEnumerator DeactiveAfterTime(float time)
    {
        var wait = new WaitForSeconds(time);
        yield return wait;
        this.gameObject.SetActive(false);
    }

    public void GetMouseTouch()
    {
        {
                if (Input.GetMouseButton(0))
                {
                    _tap = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _tap = false;
                }
        }
    }

   public void GetTouch()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Moved || Input.GetMouseButton(0))
            {
                _tap = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled || Input.GetMouseButtonUp(0))
            {
                _tap = false;
            }
        }
    }

    public Vector2 GetDirection()
    {
        if(_tap)
        {
            var touchDir = Input.touches[0].deltaPosition.normalized;

            touchDir.x = Mathf.RoundToInt(touchDir.x);
            touchDir.y = Mathf.RoundToInt(touchDir.y);

            return touchDir.normalized;
        }

        return Vector2.zero;
    }

    public Vector2 GetMouseDirection()
    {
        if (_tap)
        {
            var mouseDir = Vector2.zero;

            mouseDir.x = Mathf.RoundToInt(Input.GetAxis("Mouse X"));
            mouseDir.y = Mathf.RoundToInt(Input.GetAxis("Mouse Y"));

            return mouseDir.normalized;
        }

        return Vector2.zero;
    }

    public Vector2 GetKeyboardDirection()
    {
        Vector2 dir = Vector2.zero;

        dir.x = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ? 1 : 0;
        dir.y = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ? -1 : 0;

        return dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            this.GetComponentInChildren<Animator>().Play("Attack");
        }
    }

}
