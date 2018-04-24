using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerManager.Attributes
{
    public class AccessDeniedAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new AccessDeniedResult();
            }
        }        
    }

    public class AccessDeniedResult : ViewResult
    {
        public AccessDeniedResult()
        {
            ViewName = "~/Views/Shared/AccessDenied.cshtml";
        }
    }
}