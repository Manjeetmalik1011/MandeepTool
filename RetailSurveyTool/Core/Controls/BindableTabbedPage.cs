using System.Collections;
using System.Collections.Generic;


namespace Jci.RetailSurveyTool.TechnicianApp.Controls
{
    public class BindableTabbedPage : TabbedPage
    {
        public BindableTabbedPage()
        {
        }

        public static BindableProperty ChildrenListProperty =
            BindableProperty.Create<BindableTabbedPage, IList>(o => o.ChildrenList, new List<Page>(), propertyChanged: UpdateList);

        public IList ChildrenList
        {
            get { return (IList)GetValue(ChildrenListProperty); }
            set { SetValue(ChildrenListProperty, value); }
        }

        public static BindableProperty BindableCurrentPageProperty =
          BindableProperty.Create<BindableTabbedPage, Page>(o => o.BindableCurrentPage, new Page(), propertyChanged: UpdatePage);

        private static void UpdatePage(BindableObject bindable, Page oldValue, Page newValue)
        {

        }

        public Page BindableCurrentPage
        {
            get { return (Page)GetValue(BindableCurrentPageProperty); }
            set { SetValue(BindableCurrentPageProperty, value); }
        }

        private static void UpdateList(BindableObject bindable, IList oldValue, IList newValue)
        {
            var tabbedPage = bindable as BindableTabbedPage;
            if (tabbedPage != null)
            {
                tabbedPage.Children.Clear();
                foreach (var page in newValue)
                {
                    tabbedPage.Children.Add((Page)page);
                }
            }
        }
        string currentPageName = "";
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            BindableCurrentPage = CurrentPage;
        }

    }

}
