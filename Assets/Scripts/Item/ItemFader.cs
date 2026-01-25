using DG.Tweening;
using UnityEngine;



namespace MFarm.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemFader : MonoBehaviour
    {

        private SpriteRenderer spriteRenderer;
        // Start is called before the first frame update
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FadeIn()
        {
            Color targetColor = new Color(1, 1, 1, 1);
            spriteRenderer.DOColor(targetColor, Settings.fadeDuration);
        }

        public void FadeOut()
        {
            Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
            spriteRenderer.DOColor(targetColor, Settings.fadeDuration);
        }
    }

}
