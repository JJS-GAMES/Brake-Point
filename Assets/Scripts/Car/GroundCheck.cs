using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGround { get; private set; }
    private int _groundContacts = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _groundContacts++;
        IsGround = _groundContacts > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _groundContacts--;
        IsGround = _groundContacts > 0;
    }

}
