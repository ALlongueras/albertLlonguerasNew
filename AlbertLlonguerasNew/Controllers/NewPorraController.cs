using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AlbertLlonguerasNew.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace AlbertLlonguerasNew.Controllers
{
    public class NewPorraController : Umbraco.Web.Mvc.SurfaceController
    {
        //
        // GET: /NewPorra/

        [HttpPost]
        public ActionResult CreatePorra(NewPorraModel model)
        {
            
            //model not valid, do not save, but return current umbraco page
            if (!ModelState.IsValid)
            {
                //Perhaps you might want to add a custom message to the ViewBag
                //which will be available on the View when it renders (since we're not 
                //redirecting)          
                TempData["PorraSuccess"] = false;

                return CurrentUmbracoPage();
            }

            //Add a message in TempData which will be available 
            //in the View after the redirect 

            TempData["LocalTeam"] = model.LocalTeam;
            TempData["LocalScore"] = model.LocalScore;
            TempData["VisitorTeam"] = model.VisitorTeam;
            TempData["VisitorScore"] = model.VisitorScore;

            if (TempData["Player"].ToString()=="master")
            {
                TempData["MatchSuccess"] = true;
                return RedirectToCurrentUmbracoPage();
            }

            if (Library.Helpers.Utils.HasPorraAccordingIdentifier((IPublishedContent)TempData["PorraNode"], (string)TempData["MatchIdentifier"]))
            {
                TempData["ErrorLog"] = "Porra already done";
                TempData["PorraSuccess"] = true;
                return RedirectToCurrentUmbracoPage();
            }

            TempData["PorraSuccess"] = true;

            //redirect to current page to clear the form
            return RedirectToCurrentUmbracoPage();
        }

    }
}
