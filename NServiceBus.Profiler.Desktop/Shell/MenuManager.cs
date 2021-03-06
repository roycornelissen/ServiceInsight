using System.Windows;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using DevExpress.Xpf.Bars;
using NServiceBus.Profiler.Desktop.ExtensionMethods;
using NServiceBus.Profiler.Desktop.MessageList;
using ActionFactory = Caliburn.PresentationFramework.Actions.Action;

namespace NServiceBus.Profiler.Desktop.Shell
{
    public class MenuManager : IMenuManager
    {
        private readonly IShellView _shell;

        public MenuManager(IShellView shell)
        {
            _shell = shell;
        }

        public void CreateContextMenu(FrameworkElement view, IObservableCollection<ContextMenuModel> contextMenuItems)
        {
            var contextMenu = new PopupMenu { Manager = _shell.GetMenuManager() };
            BarManager.SetDXContextMenu(view, contextMenu);

            foreach (var item in contextMenuItems)
            {
                AddContextMenuItem(contextMenu, item);
            }
        }

        public void AddContextMenuItem(PopupMenu contextMenu, ContextMenuModel item)
        {
            var menu = new BarButtonItem
            {
                Name = item.Name, 
                Content = item.DisplayName,
                Glyph = item.Image.ToBitmapImage(),
            };

            SetUpActionHandler(menu, item);
            contextMenu.ItemLinks.Add(menu);
        }

        private void SetUpActionHandler(BarItem menu, ContextMenuModel item)
        {
            Message.SetAttach(menu, string.Format("[Event ItemClick]=[Action {0}]", item.Name));
            ActionFactory.SetTarget(menu, item.Owner);
        }
    }
}