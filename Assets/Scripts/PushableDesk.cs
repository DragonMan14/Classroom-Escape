using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableDesk : MonoBehaviour, IInteractable
{
    private GameObject player;
    public float pushSpeed = 1f;
    public bool currentlyPushing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        this.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void Interact()
    {
        if (!currentlyPushing) {
            EnablePushPullState();
        }
        else
        {
            DisablePushPullState();
        }
    }
    
    bool IsRelativePositionHorizontal()
    {
        return this.gameObject.HasTag("Horizontal");

    }

    float GetHorizontalDistance(Vector3 currentObject, Vector3 other)
    {
        return currentObject.x - other.x;
    }

    float GetVerticalDistance(Vector3 currentObject, Vector3 other)
    {
        return currentObject.y - other.y;
    }

    IEnumerator MoveDesk(bool isHorizontal)
    {
        Rigidbody2D desk = this.transform.parent.GetComponent<Rigidbody2D>();
        float horizontalDistance = GetHorizontalDistance(player.transform.position, this.transform.position);
        float verticalDistance = GetVerticalDistance(player.transform.position, this.transform.position);
        while (currentlyPushing)
        {
            if (isHorizontal)
            {
                RestrictPlayerMovement(isHorizontal, horizontalDistance);
                Vector3 newPosition = player.transform.position - new Vector3(horizontalDistance, 0.2f, 0);
                desk.MovePosition(newPosition);
            }
            else
            {
                RestrictPlayerMovement(isHorizontal, verticalDistance);
                Vector3 newPosition = player.transform.position - new Vector3(0, verticalDistance, 0);
                desk.MovePosition(newPosition);
            }
            yield return null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            return;
        }
        this.transform.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            return;
        }
        this.transform.gameObject.layer = LayerMask.NameToLayer("Player Noncollision");
    }

    void EnablePushPullState()
    {
        currentlyPushing = true;
        this.transform.parent.gameObject.layer = LayerMask.NameToLayer("Player Noncollision");
        this.transform.parent.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        PlayerAttributes attributes = player.GetComponent<PlayerAttributes>();
        bool isHorizontal = IsRelativePositionHorizontal();
        if (isHorizontal)
        {
            attributes.canMoveVertical = false;
        }
        else
        {
            attributes.canMoveHorizontal = false;
        }
        attributes.SetSpeed(pushSpeed);
        StartCoroutine(MoveDesk(isHorizontal));
    }

    void DisablePushPullState()
    {
        currentlyPushing = false;
        this.transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
        this.transform.parent.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        PlayerAttributes attributes = player.GetComponent<PlayerAttributes>();
        attributes.canMoveHorizontal = true;
        attributes.canMoveVertical = true;
        attributes.SetSpeed(attributes.walkingSpeed);
        ResetPlayerMovement();
    }

    void RestrictPlayerMovement(bool isHorizontal, float maxDistance)
    {
        ResetPlayerMovement();
        PlayerAttributes attributes = player.GetComponent<PlayerAttributes>();
        maxDistance = Mathf.Abs(maxDistance);
        if (isHorizontal)
        {
            float distance = GetHorizontalDistance(player.transform.position, this.transform.parent.transform.position);
            if (Mathf.Abs(distance) > maxDistance + 0.1f)
            {
                if (distance < 0)
                {
                    attributes.canMoveLeft = false;
                }
                else if (distance > 0)
                {
                    attributes.canMoveRight = false;
                }
            }
        }
        else
        {
            float distance = GetVerticalDistance(player.transform.position, this.transform.parent.transform.position);
            if (Mathf.Abs(distance) > maxDistance + 0.1f)
            {
                if (distance < 0)
                {
                    attributes.canMoveDown = false;
                }
                else if (distance > 0)
                {
                    attributes.canMoveUp = false;
                }
            }
        }
    }

    void ResetPlayerMovement()
    {
        PlayerAttributes attributes = player.GetComponent<PlayerAttributes>();
        attributes.canMoveDown = true;
        attributes.canMoveUp = true;   
        attributes.canMoveLeft = true;
        attributes.canMoveRight = true;
    }
}
