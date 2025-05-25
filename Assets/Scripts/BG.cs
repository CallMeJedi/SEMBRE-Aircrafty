using UnityEngine;

public class BG : MonoBehaviour
{
    private Rigidbody bgrb;
    [SerializeField] float bgSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
        {
            bgrb = GetComponent<Rigidbody>();
        }
    
    void Start()
    {
        bgrb.AddForce(bgSpeed,0,0);
    }

    
}
