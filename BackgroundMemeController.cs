using UnityEngine;
using System.Collections.Generic;


public class BackgroundMemeController : MonoBehaviour
{
    // Spawn memes in the background in a grid
    [SerializeField] private int memeWidth = 10;    // How many pixels wide should the images be
    [SerializeField] private int memeHeight = 10;   // How many pixels tall should the images be

    [SerializeField] private int startingSize;  // How big the starting grid of memes will be
                                                // in each direction (left, up, right, and down)

    [SerializeField] private int instantiateDistance = 12;

    [SerializeField] private Transform playerTransform;

    private Sprite[] memes;     // The memes we want to use for the background
    private Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>(); // The memes we have spawned

    private int minPositionLoadedX = 0;
    private int minPositionLoadedY = 0;

    private int maxPositionLoadedX = 0;
    private int maxPositionLoadedY = 0;

    void Start()
    {
        // Set up all the memes
        memes = Resources.LoadAll<Sprite>("BackgroundMemes");

        InstantiateMemes(startingSize, startingSize, Vector3.zero);
    }

    void Update()
    {

        // Check if player is close enough towards the edges to start spawning memes

        if (playerTransform != null)
        {
            if (playerTransform.position.x / memeWidth <= minPositionLoadedX + instantiateDistance * memeWidth || playerTransform.position.x / memeWidth >= maxPositionLoadedX - instantiateDistance * memeWidth
             || playerTransform.position.y / memeHeight <= minPositionLoadedY + instantiateDistance * memeHeight || playerTransform.position.y / memeHeight >= maxPositionLoadedY - instantiateDistance * memeHeight)
            {
                InstantiateMemes(instantiateDistance, instantiateDistance, new Vector2(playerTransform.position.x, playerTransform.position.y));
            }
        }

    }

    void InstantiateMemes(int width, int height, Vector2 origin)
    {
        int originX = Mathf.FloorToInt(origin.x);
        int originY = Mathf.FloorToInt(origin.y);

        for (int x = originX - width; x < originX + width; x++)
        {
            for (int y = originY - height; y < originY + height; y++)
            {
                if (!tiles.ContainsKey(new Vector2(x,y)))
                {
                    if (x < minPositionLoadedX)
                    {
                        minPositionLoadedX = x;
                    }

                    if (x > maxPositionLoadedX)
                    {
                        maxPositionLoadedX = x;
                    }

                    if (y < minPositionLoadedY)
                    {
                        minPositionLoadedY = y;
                    }

                    if (y < maxPositionLoadedX)
                    {
                        maxPositionLoadedY = y;
                    }

                    //Debug.Log("Min Position Loaded X: " + minPositionLoadedX);
                    //Debug.Log("Max Position Loaded X: " + maxPositionLoadedX);
                    //Debug.Log("Min Position Loaded Y: " + minPositionLoadedY);
                    //Debug.Log("Max Position Loaded Y: " + maxPositionLoadedY);

                    //Debug.Log("Spawning meme: (" + x + ", " + y + ")");
                    GameObject go = new GameObject();
                    tiles.Add(new Vector2(x, y), go);
                    go.transform.position = new Vector3(x * memeWidth, y * memeHeight);
                    go.transform.localScale = new Vector3(memeWidth, memeHeight, 0f);
                    //Debug.Log(originX + " + " + x + " * " + memeWidth + " = " + (originX + x * memeWidth));

                    int randomMemeIndex = Random.Range(0, memes.Length);

                    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                    sr.sprite = memes[randomMemeIndex];
                    sr.sortingLayerName = "Background";
                }
            }
        }
    }
}