using Parser.Utils.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Parser.Repos.SQLConnect.MappingConfig
{
    /// <summary>
    /// Клас для автоматичного вирішення з перевизначенням вмісту посилання на схему.
    /// </summary>
    /// <typeparam name="T1">тип який треба замапити.</typeparam>
    /// <typeparam name="T2">тип в якому треба замінити значення.</typeparam>
    public class UrlResolver<T1, T2> : IMemberValueResolver<T1, T2, string, string>
    {
        private readonly AppSettings _config;

        public UrlResolver(IOptions<AppSettings> config)
        {
            _config = config.Value;
        }

        public string Resolve(T1 source, T2 destination, string sourseMember, string destMember, ResolutionContext context)
        {
            return $"{_config.AzureContainerUrl}/{sourseMember}";
        }
    }
}
