using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Confirm()
    {
        GameManager.i.currentSession.EvaluateGuess();
    }
}
