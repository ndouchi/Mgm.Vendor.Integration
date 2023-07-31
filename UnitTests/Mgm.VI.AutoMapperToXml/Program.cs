using System;
using System.Xml.Linq;
using AutoMapper;

namespace Mgm.VI.AutoMapperToXml
{
    class Program
    {
        static readonly IndentedConsole Output = new IndentedConsole();

        private static void ShowMenuItem(MenuItem menuItem)
        {
            Console.WriteLine("  {0}", menuItem.Name);
        }
        private static void ShowMenu(Menu subMenu)
        {
            Console.WriteLine("> {0}", subMenu.Name);
            Output.Indent++;
            foreach (var sm in subMenu.SubMenus)
            {
                ShowMenu(sm);
            }
            foreach (var mi in subMenu.MenuItems)
            {
                ShowMenuItem(mi);
            }
            Output.Indent--;
        }
        static void Main(string[] args)
        {
            MapInitializer.CreateMenuMap();
            var xml = XDocument.Load(@".\Menu.xml");
            var menu = Mapper.Map<XElement, Menu>(xml.Element("Menu"));
            foreach (var sm in menu.SubMenus)
            {
                ShowMenu(sm);
            }
            foreach (var mi in menu.MenuItems)
            {
                ShowMenuItem(mi);
            }
            Console.ReadLine();
        }
    }
}
