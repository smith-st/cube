using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BonusController : MonoBehaviour
{
    private Tween _tween;
    void Start()
    {
        _tween = transform.DOMoveY(transform.position.y + 0.1f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void Collect()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        _tween.Kill();
        transform.DOMoveY(transform.position.y + 10f, 1.5f).SetEase(Ease.InSine).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        _tween?.Kill();
    }
}
