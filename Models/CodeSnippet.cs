using System;
using System.Collections.Generic;
using Kentico.Kontent.Delivery.Abstractions;
using Newtonsoft.Json;

namespace Jamstack.On.Dotnet.Models
{
    public partial class CodeSnippet
    {
        [JsonProperty(CodeSnippet.CodeCodename)]
        [CodeCustomElementConverter]
        public CodeCustomElementModel CodeStructured { get; set; }
    }
}