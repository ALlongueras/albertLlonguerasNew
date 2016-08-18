using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace AlbertLlonguerasNew.Controllers
{
    public class ManageEvents:IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += ContentService_CreatePrevia;
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        void ContentService_CreatePrevia(IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            try
            {
                if (e.PublishedEntities.First().ContentType.Alias != "Previa")
                {
                    return;
                }

                if (e.PublishedEntities.First().GetValue<bool>("notified") == false)
                {
                    var createdNodeId = e.PublishedEntities.First().Id;
                    var nodeContent = UmbracoContext.Current.Application.Services.ContentService.GetById(createdNodeId);
                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    var currentModel = umbracoHelper.TypedContent(createdNodeId);
                    var matchLink = "http://albertllongueras.cat/porra";
                    var matchLinkName = "Porra penya de l'espardenya";
                    //var match = currentModel.GetPropertyValue<string>("title");
                    var match = nodeContent.GetValue<string>("title");
                    var membersList = ApplicationContext.Current.Services.MemberService.GetAllMembers();
                    foreach (var member in membersList.Where(x => x.GetValue<bool>("notifyPrevia")))
                    {
                        var email = member.Email;
                        var name = member.Name;
                        var emailManager = new EmailManager();
                        emailManager.SendEmail(name, email, matchLink, matchLinkName, match);
                    }
                    nodeContent.SetValue("notified", true);
                    UmbracoContext.Current.Application.Services.ContentService.Save(nodeContent);
                }

            }
            catch (Exception)
            {
                
            }
        }
    }
}