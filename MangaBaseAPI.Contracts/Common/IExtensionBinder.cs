using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace MangaBaseAPI.Contracts.Common
{
    public interface IExtensionBinder<T> where T : IExtensionBinder<T>
    {
        /// <summary>
        /// The method discovered by RequestDelegateFactory on types used as parameters of route
        /// handler delegates to support custom binding.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
        /// <param name="parameter">The <see cref="ParameterInfo"/> for the parameter being bound to.</param>
        /// <returns>The value to assign to the parameter.</returns>
        static abstract ValueTask<T?> BindAsync(HttpContext httpContext, ParameterInfo parameter);
    }
}
