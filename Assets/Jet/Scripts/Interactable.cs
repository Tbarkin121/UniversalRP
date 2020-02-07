using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1f;
    public Transform interactionTransform;
    public bool isFocus = false;
    Transform player;
    bool hasInteracted = false;
    public virtual void Interact()
    {
        //This method is ment to be overwritten
        Debug.Log("Interacting with " + transform.name);
    }
    void Update ()
    {
        if(isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            // Debug.Log(distance);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }
    
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }
    public void OnDefocused () //For a multiplayer situation we probably will want to check who called defocus 
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }
    void OnDrawGizmosSelected ()
    {
        if(interactionTransform == null)
            interactionTransform = transform;
            
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
