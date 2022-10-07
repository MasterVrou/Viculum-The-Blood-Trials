using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected float movementSpeed;
    protected float minAxis;
    protected float maxAxis;
    protected float time;

    protected GameObject smallLazer;
    protected GameObject bigLazer;

    protected AttackDetails attackDetails;

    [SerializeField]
    protected LayerMask whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
