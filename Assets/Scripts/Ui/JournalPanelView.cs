using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Npc;
using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Quests
{
    public class JournalPanelView : CloseablePanel
    {
        [SerializeField] private VisitorEntryView visitorEntryViewPrefab;
        [SerializeField] private Transform leftColumn;
        [SerializeField] private Transform rightColumn;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private int currentPage = 0;
        private const int ItemsPerPage = 4; // 2 элемента на колонку, всего 4 элемента на страницу
        private List<NpcData> npcLogs;
        private int totalPages;

        private void OnEnable()
        {
            nextButton.onClick.AddListener(NextPage);
            previousButton.onClick.AddListener(PreviousPage);

            npcLogs = NpcFactory.HeroLogs.ToList();
            totalPages = Mathf.CeilToInt(npcLogs.Count / (float)ItemsPerPage);

            DisplayPage();
        }

        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(NextPage);
            previousButton.onClick.RemoveListener(PreviousPage);
        }

        private void DisplayPage()
        {
            // Очищаем предыдущий контент
            foreach (Transform child in leftColumn)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in rightColumn)
            {
                Destroy(child.gameObject);
            }

            // Вычисляем элементы для текущей страницы
            int startIdx = currentPage * ItemsPerPage;
            int endIdx = Mathf.Min(startIdx + ItemsPerPage, npcLogs.Count);

            for (int i = startIdx; i < endIdx; i++)
            {
                Transform parentColumn = i % 2 == 0 ? leftColumn : rightColumn; // Чередуем колонки
                var journalView = Instantiate(visitorEntryViewPrefab, Vector2.zero, Quaternion.identity, parentColumn);
                journalView.SetVisitor(npcLogs[i]);
            }

            // Обновляем видимость кнопок
            previousButton.gameObject.SetActive(currentPage > 0);
            nextButton.gameObject.SetActive(currentPage < totalPages - 1);
        }

        private void NextPage()
        {
            if (currentPage < totalPages - 1)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                DisplayPage();
            }
        }
    }
}