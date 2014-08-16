using System.Web.Mvc;
using AlbertLlonguerasNew.Models;

namespace AlbertLlonguerasNew.Controllers
{
    public class NewResultController : Umbraco.Web.Mvc.SurfaceController
    {
        //
        // GET: /NewResult/

        [HttpPost]
        public ActionResult CreateResult(NewResultModel model)
        {

            //model not valid, do not save, but return current umbraco page
            if (!ModelState.IsValid)
            {
                //Perhaps you might want to add a custom message to the ViewBag
                //which will be available on the View when it renders (since we're not 
                //redirecting)          
                TempData["ResultSuccess"] = false;

                return CurrentUmbracoPage();
            }

            //Add a message in TempData which will be available 
            //in the View after the redirect 

            TempData["LocalTeam"] = model.LocalTeam;
            TempData["LocalScore"] = model.LocalScore;
            TempData["VisitorTeam"] = model.VisitorTeam;
            TempData["VisitorScore"] = model.VisitorScore;
            TempData["FinalOfMonth"] = model.FinalOfMonth;
            //model.PorraNode=Umbraco.TypedContent(1151);
            //if (TempData["Player"].ToString() == "master")
            //{
            //    TempData["ResultSuccess"] = true;
            //    return RedirectToCurrentUmbracoPage();
            //}

            //if (Library.Helpers.Utils.HasPorraAccordingIdentifier((IPublishedContent)TempData["PorraNode"], (string)TempData["MatchIdentifier"]))
            //{
            //    TempData["ErrorLog"] = "Porra already done";
            //    TempData["PorraSuccess"] = true;
            //    return RedirectToCurrentUmbracoPage();
            //}

            TempData["ResultSuccess"] = true;

            //redirect to current page to clear the form
            return RedirectToCurrentUmbracoPage();
        }

    }
}
