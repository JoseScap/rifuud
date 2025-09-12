using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Api.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void UseGeneralRoutePrefix(this MvcOptions opts, string prefix)
        {
            opts.Conventions.Add(new RouteTokenTransformerConvention(prefix));
        }
    }

    public class RouteTokenTransformerConvention : IApplicationModelConvention
    {
        private readonly string _prefix;

        public RouteTokenTransformerConvention(string prefix)
        {
            _prefix = prefix;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var hasRouteAttribute = controller.Selectors
                    .Any(selector => selector.AttributeRouteModel != null);

                if (!hasRouteAttribute)
                {
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel
                        {
                            Template = _prefix + "/[controller]"
                        }
                    });
                }
                else
                {
                    foreach (var selector in controller.Selectors.Where(s => s.AttributeRouteModel != null))
                    {
                        if (selector.AttributeRouteModel?.Template != null && 
                            !selector.AttributeRouteModel.Template.StartsWith(_prefix))
                        {
                            selector.AttributeRouteModel.Template = 
                                _prefix + "/" + selector.AttributeRouteModel.Template.TrimStart('/');
                        }
                    }
                }
            }
        }
    }
}