using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }


    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;

    public float jumpValue = 0.0f;
    public bool canJump = true;

    private float HorizontalInput;
    private bool fly = false;

    private Rigidbody2D gravity;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private Transform transf;

    public PhysicsMaterial2D bounceMat, normalMat;

    private void Awake()
    {
        gravity = GetComponent<Rigidbody2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        transf = GetComponent<Transform>();
    }

    private void Update()
    {

        //Hrac nebude moct rotovat (ak bude na rohu platformy tak sa charakter neprekoti

        transf.rotation = Quaternion.identity;

        //Nastavenie gravitacie na 4.35

        gravity.gravityScale = 4.35f;
        HorizontalInput = Input.GetAxis("Horizontal");

        if(isGrounded())
        {
            canJump = true;
            fly = false;
        }

        if(!isGrounded())
        {
            canJump = false;
            fly = true;
            body.sharedMaterial = bounceMat;
        }

        if(!isGrounded() && Input.GetKeyUp("space"))
        {
            canJump = true;
        }

        if (jumpValue == 0.0f && isGrounded() && fly == false)
        {
            body.velocity = new Vector2((HorizontalInput) * speed, body.velocity.y);
            print(body.velocity);
        }

        //Otocenie hraca doprava alebo dolava zeleziac do akej strany hrac pojde

        if (HorizontalInput > 0.01f)
            transform.localScale = new Vector3(2,2,2);

        else if(HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-2, 2, 2);

        //Ak stalcime space a hrac je momentalne na zemi a ma dovolene skakat tak skoci

        if (Input.GetKey("space") && isGrounded() && canJump)
        {
            buildJump();
        }

        //Ak si hrac naboostoval maximalny jump tak je nuteny skocit

        if (jumpValue >= 29f && isGrounded())
        {
            forceJump();
        }

        //ak hrac v priebehu boostovania jumpu da hore space tak skoci s urcitou vyskou

        if(Input.GetKeyUp(KeyCode.Space))
        {
            releasedSpaceJump();
        }

        //Animacie hraca//

        anim.SetBool("Run", HorizontalInput != 0 && !Input.GetKeyDown("space"));
        anim.SetBool("Grounded", isGrounded());

        //Hrac nemoze utekat ak drzi space

        if(Input.GetKeyDown("space") && isGrounded() && canJump)
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }

        if(body.velocity.y <= -1)
        {

            body.sharedMaterial = normalMat;
        }
    }


    //Funkcia zvysuje jumpValue, ktora sa neskor pouziva na vypocitanie ako vysoko a daleko ma hrac skocit

    private void buildJump()
    {
        jumpValue += 0.2f;
        body.sharedMaterial = bounceMat;
    }

    //Ak hracov jumpValue je maximalny tak sa vola funkcia ktora prida do body.velocity hodnoty a tym hrac automaticky skoci

    private void forceJump()
    {
        float pX = HorizontalInput * speed;
        float pY = jumpValue;

        body.velocity = new Vector2(pX, pY);
        anim.SetTrigger("Jump");
        body.sharedMaterial = bounceMat;
        Debug.Log("A");
        //Vbudovana funkcia ktora sa vola po 0.2s
        Invoke("resetJump", 0.1f);
    }

    private void releasedSpaceJump()
    {
        if (isGrounded())
        {
            body.sharedMaterial = bounceMat;
            body.velocity = new Vector2(HorizontalInput * speed, jumpValue);
            jumpValue = 0.0f;
            anim.SetTrigger("Jump");
            Debug.Log("B");
        }

        canJump = true;
    }

    //Funkcia ak hrac skoci tak sa vola a vyresetuje jumpValue na 0 aby si ju musel hrac znova zvysovat.
    //Taktiez Hrac nebude moct skakat aj ked je vo vzduchu

    private void resetJump()
    {
        canJump = false;
        jumpValue = 0f;
    }

    //Funkcia pozera ak je hrac na zemi tak vraca true inak false
    //Funkcia vytvori krabicu(box) pod hracom

    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}

