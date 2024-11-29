using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_253501_LAVRIV.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("category")]
        public string? Category { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Добавляем отладочный вывод
            Console.WriteLine($"Current Page: {CurrentPage}, Total Pages: {TotalPages}");

            // Создаем контейнер ul с классом pagination
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            // Кнопка "предыдущая страница"
            AddPageLink(ul, "<<", CurrentPage > 1 ? CurrentPage - 1 : 1, CurrentPage == 1);

            // Кнопки для каждой страницы
            for (int i = 1; i <= TotalPages; i++)
            {
                AddPageLink(ul, i.ToString(), i, CurrentPage == i);
            }

            // Кнопка "следующая страница"
            AddPageLink(ul, ">>", CurrentPage < TotalPages ? CurrentPage + 1 : TotalPages, CurrentPage == TotalPages);

            // Выводим ul
            output.TagName = "nav";
            output.Content.AppendHtml(ul);

            // Проверка содержимого
            Console.WriteLine(output.Content.GetContent());
        }


        private void AddPageLink(TagBuilder ul, string text, int pageNo, bool isDisabledOrActive)
        {
            // Создаем li
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (isDisabledOrActive)
            {
                li.AddCssClass(text == CurrentPage.ToString() ? "active" : "disabled");
            }

            // Создаем ссылку
            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            a.Attributes["href"] = $"?category={Category}&pageNo={pageNo}";
            a.InnerHtml.Append(text);

            // Добавляем ссылку в li, а затем li в ul
            li.InnerHtml.AppendHtml(a);
            ul.InnerHtml.AppendHtml(li);
        }
    }
}
