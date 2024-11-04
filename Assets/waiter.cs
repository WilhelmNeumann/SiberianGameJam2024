using DG.Tweening;
using UnityEngine;

public class waiter : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        DOTween.Sequence()
            .Append(transform.DOMove(new Vector3(-20, -2, 0), 10f))
            .SetEase(Ease.Linear)
            .AppendCallback(() => transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y))
            .Append(transform.DOMove(new Vector3(20, -2, 0), 10f))
            .SetEase(Ease.Linear)
            .AppendCallback(() => transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y))
            .SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
    }
}