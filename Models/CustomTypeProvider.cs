using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Delivery.Abstractions;

namespace Jamstack.On.Dotnet.Models
{
    public class CustomTypeProvider : ITypeProvider
    {
        private static readonly Dictionary<Type, string> _codenames = new Dictionary<Type, string>
        {
            {typeof(ColumnsComponent), "columns_component"},
            {typeof(Cta), "cta"},
            {typeof(ImageAsset), "image_asset"},
            {typeof(LandingPage), "landing_page"},
            {typeof(Page), "page"},
            {typeof(Root), "root"},
            {typeof(RowsComponent), "rows_component"},
            {typeof(Text), "text"}
        };

        public Type GetType(string contentType)
        {
            return _codenames.Keys.FirstOrDefault(type => GetCodename(type).Equals(contentType));
        }

        public string GetCodename(Type contentType)
        {
            return _codenames.TryGetValue(contentType, out var codename) ? codename : null;
        }
    }
}