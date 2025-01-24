using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    public class Toast : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI messageText;

        private float showTimer;
        private RectTransform containerRect;

        private static Toast instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            containerRect = messageText.transform.parent.GetComponent<RectTransform>();
            canvasGroup.alpha = 0;
        }

        void Update()
        {
            showTimer = Mathf.Max(showTimer - Time.deltaTime, 0);

            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, showTimer == 0 ? 0 : 1, Time.deltaTime * 5);
        }

        public static void Show(string message)
        {
            instance.messageText.text = message;
            LayoutRebuilder.ForceRebuildLayoutImmediate(instance.containerRect);
            instance.showTimer = 4f;
        }
    }
}

