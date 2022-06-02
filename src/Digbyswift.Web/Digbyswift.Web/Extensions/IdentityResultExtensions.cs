using System;
using System.Linq;
using Digbyswift.Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace Digbyswift.Web.Extensions
{
    public static class IdentityResultExtensions
    {
        public static string ErrorsAsString(this IdentityResult result, string delimiter = StringConstants.Colon, bool includeCode = false)
        {
            if (includeCode)
                return String.Join(delimiter, result.Errors.Select(x => $"{x.Description} ({x.Code})"));

            return String.Join(delimiter, result.Errors.Select(x => x.Description));
        }
    }
}