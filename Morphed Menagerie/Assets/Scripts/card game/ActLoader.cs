using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;

public class ActLoader : MonoBehaviour
{
    public static ActLoader Instance { get; private set; }

    [Header("Acts")]
    public Act currentAct = Act.Menu;
    public Act loadAct = Act.Menu;
    public TextMeshProUGUI loadableAct;

    [Header("Character decks")]
    public CharacterDeck playerRini;
    public CharacterDeck playerFao, rini, fao;

    public Scenes sceneCollection;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        loadableAct.text = "Load: " + loadAct.ToString();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(sceneCollection.menu.name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            loadableAct.gameObject.SetActive(!loadableAct.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // prev
            switch (loadAct)
            {
                default:
                    loadAct = Act.Menu;
                    break;
                case Act.Menu:
                    loadAct = Act.Ending;
                    break;
                case Act.ZooRini:
                    loadAct = Act.Menu;
                    break;
                case Act.Office:
                    loadAct = Act.ZooRini;
                    break;
                case Act.RiniIntro:
                    loadAct = Act.Office;
                    break;
                case Act.RiniBattle:
                    loadAct = Act.RiniIntro;
                    break;
                case Act.RiniOutro:
                    loadAct = Act.RiniBattle;
                    break;
                case Act.ZooFao:
                    loadAct = Act.RiniOutro;
                    break;
                case Act.FaoBattle:
                    loadAct = Act.ZooFao;
                    break;
                case Act.FaoOutro:
                    loadAct = Act.FaoBattle;
                    break;
                case Act.Ending:
                    loadAct = Act.FaoOutro;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // next
            switch (loadAct)
            {
                default:
                    loadAct = Act.Menu;
                    break;
                case Act.Menu:
                    loadAct = Act.ZooRini;
                    break;
                case Act.ZooRini:
                    loadAct = Act.Office;
                    break;
                case Act.Office:
                    loadAct = Act.RiniIntro;
                    break;
                case Act.RiniIntro:
                    loadAct = Act.RiniBattle;
                    break;
                case Act.RiniBattle:
                    loadAct = Act.RiniOutro;
                    break;
                case Act.RiniOutro:
                    loadAct = Act.ZooFao;
                    break;
                case Act.ZooFao:
                    loadAct = Act.FaoBattle;
                    break;
                case Act.FaoBattle:
                    loadAct = Act.FaoOutro;
                    break;
                case Act.FaoOutro:
                    loadAct = Act.Ending;
                    break;
                case Act.Ending:
                    loadAct = Act.Menu;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Loading: " + loadAct.ToString());
            LoadAct(loadAct);
        }
    }

    public void LoadAct(Act actToLoad)
    {
        currentAct = actToLoad;
        switch (currentAct)
        {
            default:
                break;
            case Act.Menu:
                SceneManager.LoadScene(sceneCollection.menu.name);
                break;
            case Act.ZooRini:
                //SceneManager.LoadScene(sceneCollection.zooRini.name);
                SceneManager.LoadScene(sceneCollection.zoo.name);
                break;
            case Act.Office:
                SceneManager.LoadScene(sceneCollection.office.name);
                break;
            case Act.RiniIntro:
                SceneManager.LoadScene(sceneCollection.riniIntro.name);
                break;
            case Act.RiniBattle:
                SceneManager.LoadScene(sceneCollection.battle.name);
                break;
            case Act.RiniOutro:
                SceneManager.LoadScene(sceneCollection.riniOutro.name);
                break;
            case Act.ZooFao:
                //SceneManager.LoadScene(sceneCollection.zooFao.name);
                SceneManager.LoadScene(sceneCollection.zoo.name);
                break;
            case Act.FaoBattle:
                SceneManager.LoadScene(sceneCollection.battle.name);
                break;
            case Act.FaoOutro:
                //SceneManager.LoadScene(sceneCollection.faoOutro.name);
                SceneManager.LoadScene(sceneCollection.zoo.name);
                break;
            case Act.Ending:
                SceneManager.LoadScene(sceneCollection.thankyou.name);
                break;
            // This act is not in the cheat list
            case Act.BattleMode:
                SceneManager.LoadScene(sceneCollection.battlemode.name);
                break;
            case Act.ThankYou:
                SceneManager.LoadScene(sceneCollection.thankyou.name);
                break;
            case Act.KilledRini:
                SceneManager.LoadScene(sceneCollection.killrini.name);
                break;
            case Act.RiniLose:
                SceneManager.LoadScene(sceneCollection.rinilose.name);
                break;
            case Act.FaoLose:
                //SceneManager.LoadScene(sceneCollection.faolose.name);
                SceneManager.LoadScene(sceneCollection.zoo.name);
                break;
            case Act.FaoNoSacri:
                //SceneManager.LoadScene(sceneCollection.nokill.name);
                SceneManager.LoadScene(sceneCollection.zoo.name);
                break;
        }
    }
}

public enum Act
{
    Menu,
    ZooRini,
    Office,
    RiniIntro,
    RiniBattle,
    RiniOutro,
    ZooFao,
    FaoBattle,
    FaoOutro,
    Ending,
    BattleMode,
    ThankYou,
    KilledRini,
    RiniLose,
    FaoLose,
    FaoNoSacri
}

[System.Serializable]
public struct Scenes
{
    [Header("General")]
    public SceneAsset menu;
    public SceneAsset battle, thankyou, zoo, battlemode; // TODO: zoo generic as phase 2
    [Header("Rini")]
    public SceneAsset zooRini, office;
    public SceneAsset riniIntro, riniOutro, rinilose; // TODO: make win & lose scenes part of character scene
    [Header("Fao")]
    public SceneAsset zooFao;
    public SceneAsset faoIntro, faoOutro, faolose, killrini, nokill;
}
