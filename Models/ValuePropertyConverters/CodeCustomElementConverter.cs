using Kentico.Kontent.Delivery.Abstractions;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jamstack.On.Dotnet.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CodeCustomElementConverter : Attribute, IPropertyValueConverter<string>
    {
        public Task<object> GetPropertyValueAsync<TElement>(PropertyInfo property, TElement element, ResolvingContext context) where TElement : IContentElementValue<string>
        {
            if (String.IsNullOrEmpty(element.Value))
            {
                return Task.FromResult((object)element.Value);
            }



            var model = JsonSerializer.Deserialize<CodeCustomElementModel>(element.Value);
            return Task.FromResult((object)model);
        }
    }
}
