using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// represents a gameObject that contains an Item;
public class ItemGameObject : MonoBehaviour
{
    public string playerTag;
    public Item Item;
    public int count;

    private ItemStack stack;

    private Rigidbody2D rb;

    bool isCollected = false;
    SpriteRenderer spriteRenderer;

    [Header("Sounds")]
    [SerializeField] AudioClip[] pickUpClips;

    [Header("Highlight near player")]
    [SerializeField] float hightlightRadius = 5f;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material nearPlayerMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (count <= 0)
        {
            count = 1;
        }

        stack = new ItemStack(Item, count);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Item.sprite;
        spriteRenderer.material = defaultMaterial;
    }

    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag(playerTag);
        if (target != null && Vector2.Distance(transform.position, target.transform.position) < hightlightRadius)
        {
            spriteRenderer.material = nearPlayerMaterial;
        }
        else
        {
            spriteRenderer.material = defaultMaterial;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inventory = collision.gameObject.GetComponentInParent<Inventory>();


        if (!isCollected && inventory != null && inventory.HasCapacityForItem(Item))
        {
            isCollected = true;
            // RandomSound.PlayClipAtPoint(pickUpClips, transform.position);
            if (inventory.addItemSource != null)
            {
                RandomSound.Play(inventory.addItemSource, pickUpClips);
            }

            Pickup(inventory);
            StartCoroutine(DespawnSequence());
        }
    }

    void Pickup(Inventory inventory)
    {
        // detect an inventory and add <count> of this item to that number
        inventory.AddItem(stack);
    }

    private IEnumerator DespawnSequence()
    {
        yield return null;
        Destroy(gameObject);
    }
}
