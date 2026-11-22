using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    [Header("Book User Interface")]
    [SerializeField] private GameObject bookPanel;
    [SerializeField] private Image pageDisplay;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text pageCounter;
    [SerializeField] private TMP_Text missingPageMessage;

    [Header("Book Pages in Correct Order")]
    [SerializeField] private Sprite[] pageSprites = new Sprite[5];

    [Header("Book Settings")]
    [SerializeField] private bool pauseGameWhenBookIsOpen = true;

    private bool[] unlockedPages;
    private int currentPageIndex;

    private const int PageCount = 5;
    private const string SaveKeyPrefix = "BookPage_";

    private void Awake()
    {
        // Make sure there is only one BookManager instance.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Create the array that stores the unlocked state of each page.
        unlockedPages = new bool[PageCount];

        LoadProgress();
    }

    private void Start()
    {
        // Connect the navigation buttons to their corresponding methods.
        previousButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);

        // Start the game with the book closed and the first page selected.
        bookPanel.SetActive(false);
        currentPageIndex = 0;

        RefreshPage();
    }

    /// <summary>
    /// Opens the book interface.
    /// </summary>
    public void OpenBook()
    {
        bookPanel.SetActive(true);
        RefreshPage();

        // Pause gameplay while the book is open.
        if (pauseGameWhenBookIsOpen)
        {
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// Closes the book interface.
    /// </summary>
    public void CloseBook()
    {
        bookPanel.SetActive(false);

        // Resume gameplay after closing the book.
        if (pauseGameWhenBookIsOpen)
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Moves to the next page if one exists.
    /// </summary>
    public void NextPage()
    {
        if (currentPageIndex >= PageCount - 1)
        {
            return;
        }

        currentPageIndex++;
        RefreshPage();
    }

    /// <summary>
    /// Moves to the previous page if one exists.
    /// </summary>
    public void PreviousPage()
    {
        if (currentPageIndex <= 0)
        {
            return;
        }

        currentPageIndex--;
        RefreshPage();
    }

    /// <summary>
    /// Unlocks a specific book page and saves its state.
    /// </summary>
    /// <param name="pageIndex">
    /// The page index, starting at zero.
    /// </param>
    public void UnlockPage(int pageIndex)
    {
        // Prevent access to an invalid array position.
        if (pageIndex < 0 || pageIndex >= PageCount)
        {
            Debug.LogWarning("Invalid book page index: " + pageIndex);
            return;
        }

        // Do not unlock the same page more than once.
        if (unlockedPages[pageIndex])
        {
            return;
        }

        unlockedPages[pageIndex] = true;

        // Save the unlocked page immediately.
        PlayerPrefs.SetInt(SaveKeyPrefix + pageIndex, 1);
        PlayerPrefs.Save();

        // Select the newly unlocked page.
        currentPageIndex = pageIndex;

        Debug.Log("Book page unlocked: " + (pageIndex + 1));

        // Refresh the interface only if the book is currently open.
        if (bookPanel.activeSelf)
        {
            RefreshPage();
        }
    }

    /// <summary>
    /// Returns true if the requested page has already been unlocked.
    /// </summary>
    public bool IsPageUnlocked(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= PageCount)
        {
            return false;
        }

        return unlockedPages[pageIndex];
    }

    /// <summary>
    /// Updates the current page image, page counter, messages,
    /// and navigation buttons.
    /// </summary>
    private void RefreshPage()
    {
        bool isCurrentPageUnlocked = unlockedPages[currentPageIndex];

        // Update the page counter displayed to the player.
        pageCounter.text =
            " PAGINA " + (currentPageIndex + 1) + " DE " + PageCount;

        // Show the page image only when the page is unlocked.
        pageDisplay.gameObject.SetActive(isCurrentPageUnlocked);

        // Show the missing-page message when the page is locked.
        missingPageMessage.gameObject.SetActive(!isCurrentPageUnlocked);

        if (isCurrentPageUnlocked)
        {
            if (pageSprites[currentPageIndex] != null)
            {
                pageDisplay.sprite = pageSprites[currentPageIndex];
                pageDisplay.preserveAspect = true;
            }
            else
            {
                Debug.LogWarning(
                    "No sprite was assigned to book page " +
                    (currentPageIndex + 1)
                );
            }
        }

        // Disable navigation buttons when reaching the first or last page.
        previousButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < PageCount - 1;
    }

    /// <summary>
    /// Loads the unlocked pages from PlayerPrefs.
    /// </summary>
    private void LoadProgress()
    {
        for (int i = 0; i < PageCount; i++)
        {
            unlockedPages[i] =
                PlayerPrefs.GetInt(SaveKeyPrefix + i, 0) == 1;
        }

        // These pages are available from the beginning of the game.
        unlockedPages[0] = true; // Page 1: A-E
        unlockedPages[2] = true; // Page 3: K-Ñ
    }

    /// <summary>
    /// Deletes all saved book progress.
    /// This method can also be executed from the component context menu.
    /// </summary>
    [ContextMenu("Reset Book Progress")]
    public void ResetBookProgress()
    {
        for (int i = 0; i < PageCount; i++)
        {
            PlayerPrefs.DeleteKey(SaveKeyPrefix + i);
        }

        PlayerPrefs.Save();

        // Restore the starting configuration.
        LoadProgress();
        currentPageIndex = 0;
        RefreshPage();

        Debug.Log("Book progress was successfully reset.");
    }

    private void OnDestroy()
    {
        // Remove button listeners when this component is destroyed.
        if (previousButton != null)
        {
            previousButton.onClick.RemoveListener(PreviousPage);
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(NextPage);
        }

        // Restore normal game speed if the object is destroyed
        // while the book is open.
        if (pauseGameWhenBookIsOpen)
        {
            Time.timeScale = 1f;
        }
    }
}