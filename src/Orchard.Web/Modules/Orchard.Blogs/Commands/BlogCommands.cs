﻿using System;
using System.Linq;
using System.Xml.Linq;
using Orchard.Blogs.Models;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Navigation.Models;
using Orchard.Security;
using Orchard.Blogs.Services;
using Orchard.Core.Navigation.Services;
using Orchard.Settings;
using Orchard.Core.Title.Models;
using Orchard.UI.Navigation;
using Orchard.Utility;

namespace Orchard.Blogs.Commands {
    public class BlogCommands : DefaultOrchardCommandHandler {
        private readonly IContentManager _contentManager;
        private readonly IMembershipService _membershipService;
        private readonly IBlogService _blogService;
        private readonly IMenuService _menuService;
        private readonly ISiteService _siteService;
        private readonly INavigationManager _navigationManager;

        public BlogCommands(
            IContentManager contentManager,
            IMembershipService membershipService,
            IBlogService blogService,
            IMenuService menuService,
            ISiteService siteService,
            INavigationManager navigationManager) {
            _contentManager = contentManager;
            _membershipService = membershipService;
            _blogService = blogService;
            _menuService = menuService;
            _siteService = siteService;
            _navigationManager = navigationManager;
        }

        [OrchardSwitch]
        public string FeedUrl { get; set; }

        [OrchardSwitch]
        public int BlogId { get; set; }

        [OrchardSwitch]
        public string Owner { get; set; }

        [OrchardSwitch]
        public string Slug { get; set; }

        [OrchardSwitch]
        public string Title { get; set; }

        [OrchardSwitch]
        public string Description { get; set; }

        [OrchardSwitch]
        public string MenuText { get; set; }

        [OrchardSwitch]
        public string MenuName { get; set; }

        [OrchardSwitch]
        public bool Homepage { get; set; }

        [CommandName("blog create")]
        [CommandHelp("blog create [/Slug:<slug>] /Title:<title> [/Owner:<username>] [/Description:<description>] [/MenuName:<name>] [/MenuText:<menu text>] [/Homepage:true|false]\r\n\t" + "Creates a new Blog")]
        [OrchardSwitches("Title,Owner,Description,MenuText,Homepage,MenuName")]
        public void Create() {
            if (String.IsNullOrEmpty(Owner)) {
                Owner = _siteService.GetSiteSettings().SuperUser;
            }
            var owner = _membershipService.GetUser(Owner);

            if (owner == null) {
                Context.Output.WriteLine(T("Invalid username: {0}", Owner));
                return;
            }

            var blog = _contentManager.New("Blog");
            blog.As<ICommonPart>().Owner = owner;
            blog.As<TitlePart>().Title = Title;
            if (!String.IsNullOrEmpty(Description)) {
                blog.As<BlogPart>().Description = Description;
            }
            
            if ( !String.IsNullOrWhiteSpace(MenuText) ) {
                var menu = _menuService.GetMenu(MenuName);

                if (menu != null) {
                    var menuItem = _contentManager.Create<ContentMenuItemPart>("ContentMenuItem");
                    menuItem.Content = blog;
                    menuItem.As<MenuPart>().MenuPosition = Position.GetNext(_navigationManager.BuildMenu(menu));
                    menuItem.As<MenuPart>().MenuText = MenuText;
                    menuItem.As<MenuPart>().Menu = menu;
                }
            }

            if (Homepage || !String.IsNullOrWhiteSpace(Slug)) {
                dynamic dblog = blog;
                if (dblog.AutoroutePart != null) {
                    dblog.AutoroutePart.UseCustomPattern = true;
                    dblog.AutoroutePart.CustomPattern = Homepage ? "/" : Slug;
                }
            }
            
            _contentManager.Create(blog);

            Context.Output.WriteLine(T("Blog created successfully"));
        }

        [CommandName("blog import")]
        [CommandHelp("blog import /BlogId:<id> /FeedUrl:<feed url> /Owner:<username>\r\n\t" + "Import all items from <feed url> into the blog specified by <id>")]
        [OrchardSwitches("FeedUrl,BlogId,Owner")]
        public void Import() {
            var owner = _membershipService.GetUser(Owner);

            if(owner == null) {
                Context.Output.WriteLine(T("Invalid username: {0}", Owner));
                return;
            }

            XDocument doc;

            try {
                Context.Output.WriteLine(T("Loading feed..."));
                doc = XDocument.Load(FeedUrl);
                Context.Output.WriteLine(T("Found {0} items", doc.Descendants("item").Count()));
            }
            catch (Exception ex) {
                throw new OrchardException(T("An error occured while loading the feed at {0}.", FeedUrl), ex);
            }

            var blog = _blogService.Get(BlogId, VersionOptions.Latest);

            if ( blog == null ) {
                Context.Output.WriteLine(T("Blog not found with specified Id: {0}", BlogId));
                return;
            }

            foreach ( var item in doc.Descendants("item") ) {
                if (item != null) {
                    var postName = item.Element("title").Value;

                    Context.Output.WriteLine(T("Adding post: {0}...", postName.Substring(0, Math.Min(postName.Length, 40))));
                    var post = _contentManager.New("BlogPost");
                    post.As<ICommonPart>().Owner = owner;
                    post.As<ICommonPart>().Container = blog;
                    post.As<TitlePart>().Title = postName;
                    post.As<BodyPart>().Text = item.Element("description").Value;
                    _contentManager.Create(post);
                }
            }

            Context.Output.WriteLine(T("Import feed completed."));
        }

    }
}