using UnityEngine;
using UnityEngine.UI;
public class UIManger : MonoBehaviour {
    [SerializeField]
    private Button restartBtn;

    [SerializeField]
    private Button nextBtn;

    [SerializeField]
    private Button playBtn;


    [SerializeField]
    private Button stopBtn;

    [SerializeField]
    private FindPathAStar findPathAStar;

    [SerializeField]
    private Texture2D defaultCursor;


    // Start is called before the first frame update

    void Start() {
        restartBtn.onClick.AddListener(RestartBtnOnClick);
        nextBtn.onClick.AddListener(NextBtnOnClick);
        playBtn.onClick.AddListener(StartAutoSearchBtnOnClick);
        stopBtn.onClick.AddListener(StopAutoSearchBtnOnClick);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void RestartBtnOnClick() {
        findPathAStar.BeginSearch();
    }
    void NextBtnOnClick() {
        if (!findPathAStar.autoSearch)
            findPathAStar.StepSearch(findPathAStar.lastNode);
    }

    void StopAutoSearchBtnOnClick() {
        playBtn.gameObject.SetActive(true);
        stopBtn.gameObject.SetActive(false);
        findPathAStar.autoSearch = false;
    }
    void StartAutoSearchBtnOnClick() {
        playBtn.gameObject.SetActive(false);
        stopBtn.gameObject.SetActive(true);
        findPathAStar.autoSearch = true;
        findPathAStar.StartCoroutine(findPathAStar.StartAutoSearch());
    }
}
