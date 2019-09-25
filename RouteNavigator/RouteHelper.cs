using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace RouteNavigator
{
    internal static class RouteHelper
    {
        public static List<Route> GetAllRoutes(string path)
        {
            using (AssemblyDefinition myLibrary = AssemblyDefinition.ReadAssembly(path))
            {
                var allRoutes = new List<Route>();
                foreach (TypeDefinition type in myLibrary.MainModule.Types)
                {
                    var routes = GetRoutes(type);
                    if (routes != null) allRoutes.AddRange(routes);
                }
                allRoutes = (from r in allRoutes orderby r.Prefix, r.Template select r).ToList();
                return allRoutes;
            }
        }

        static List<Route> GetRoutes(TypeDefinition type)
        {
            var routes = new List<Route>();

            var routePrefix = GetRoutePrefix(type);
            if (routePrefix == null) return routes;

            foreach (var method in type.Methods)
            {
                string route = GetRoute(method);
                if (route == null) continue;

                routes.Add(new Route(
                    routePrefix,
                    route,
                    type.FullName,
                    method.Name,
                    GetHttpMethod(method)
                ));
            }
            return routes;
        }

        static string GetRoutePrefix(TypeDefinition td)
        {
            var routePrefixAttr = td.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName == "System.Web.Http.RoutePrefixAttribute");
            if (routePrefixAttr == null) return null;

            if (routePrefixAttr.Constructor.Parameters.Count != 1) throw new Exception("Huh?");
            if (routePrefixAttr.Constructor.Parameters[0].ParameterType.FullName != "System.String") throw new Exception("Huh?");

            return (string)routePrefixAttr.ConstructorArguments[0].Value;
        }

        static string GetRoute(MethodDefinition method)
        {
            var routeAttr = method.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName == "System.Web.Http.RouteAttribute");
            if (routeAttr == null) return null;

            if (routeAttr.Constructor.Parameters.Count != 1) throw new Exception("Huh?");
            if (routeAttr.Constructor.Parameters[0].ParameterType.FullName != "System.String") throw new Exception("Huh?");
            if (!method.IsPublic) throw new Exception("Huh?");

            return (string)routeAttr.ConstructorArguments[0].Value;
        }

        private static string GetHttpMethod(MethodDefinition method)
        {
            var attr = method.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName.StartsWith("System.Web.Http.Http"));
            switch (attr?.AttributeType.Name)
            {
                case "HttpDeleteAttribute": return "DELETE";
                case "HttpGetAttribute": return "GET";
                case "HttpHeadAttribute": return "HEAD";
                case "HttpOptionsAttribute": return "OPTIONS";
                case "HttpPatchAttribute": return "PATCH";
                case "HttpPostAttribute": return "POST";
                case "HttpPutAttribute": return "PUT";
            }
            return "";
        }
    }
}