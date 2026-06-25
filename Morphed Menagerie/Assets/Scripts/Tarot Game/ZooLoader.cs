using System.Collections;
using UnityEngine;

public class ZooLoader : MonoBehaviour
{
    public GameObject zooRini, zooFaoIntro, zooFaoLose, zooFaoKillRini, zooFaoLiveRini;

    private void Start()
    {
        ActLoader.Instance.waitCount = 0;
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        yield return new WaitForSeconds(1f);
        ActLoader.Instance.waitCount = 0;
        ActLoader.Instance.waitLoad = true;

        yield return new WaitForSeconds(2f);
        zooRini.SetActive(false);
        zooFaoIntro.SetActive(false);
        zooFaoLose.SetActive(false);
        zooFaoKillRini.SetActive(false);
        zooFaoLiveRini.SetActive(false);

        switch (ActLoader.Instance.currentAct)
        {
            default:
                Debug.Log("Scene not assigned! " + ActLoader.Instance.currentAct.ToString());
                break;
            case Act.ZooRini:
                zooRini.SetActive(true);
                break;
            case Act.ZooFao:
                zooFaoIntro.SetActive(true);
                break;
            case Act.FaoOutro:
                zooFaoKillRini.SetActive(true);
                break;
            case Act.FaoNoSacri:
                zooFaoLiveRini.SetActive(true);
                break;
            case Act.FaoLose:
                zooFaoLose.SetActive(true);
                break;
        }
    }
}
