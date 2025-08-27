using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string playerTag;
    [SerializeField] private float highlightRadius = 5f;
    [SerializeField] private Item recipe;


    SpriteRenderer spriteRenderer;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material nearPlayerMaterial;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Inventory>(out var inventory))
        {
            if (inventory.IndexOf(recipe) >= 0)
            {
                LevelManager.WinLevel();
                Debug.Log($"[Door] {collision} has item {recipe.itemName}");
            }
            else
            {
                Debug.Log($"[Door] {collision} does NOT have item {recipe.itemName}");
            }
        }
    }

    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag(playerTag);
        if (target != null && Vector2.Distance(transform.position, target.transform.position) < highlightRadius && target.gameObject.TryGetComponent<Inventory>(out var inventory) && inventory.IndexOf(recipe) >= 0)
        {
            spriteRenderer.material = nearPlayerMaterial;
        }
        else
        {
            spriteRenderer.material = defaultMaterial;
        }
    }
}
