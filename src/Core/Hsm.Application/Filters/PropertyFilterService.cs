using Microsoft.AspNetCore.Mvc.Filters;

namespace Hsm.Application.Filters
{
    public class PropertyFilterService : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var parameters = context.ActionArguments;

            // Check and update string properties (province, district) if they exist
            if (parameters.ContainsKey("province"))
            {
                var provinceValue = parameters["province"] as string;
                if (provinceValue != null)
                {
                    parameters["province"] = CapitalizeFirstLetter(ConvertToTurkish(provinceValue));
                }
            }

            if (parameters.ContainsKey("district"))
            {
                var districtValue = parameters["district"] as string;
                if (districtValue != null)
                {
                    parameters["district"] = CapitalizeFirstLetter(ConvertToTurkish(districtValue));
                }
            }

            // Proceed with the next action in the pipeline
            await next();
        }

        // Converts English characters to Turkish equivalents
        public static string ConvertToTurkish(string input)
        {
            var englishToTurkishMap = new Dictionary<char, char>
            {
                { 'c', 'ç' }, { 'C', 'Ç' },
                { 'g', 'ğ' }, { 'G', 'Ğ' },
                { 'i', 'ı' }, { 'I', 'İ' },
                { 's', 'ş' }, { 'S', 'Ş' },
                { 'u', 'ü' }, { 'U', 'Ü' },
                { 'o', 'ö' }, { 'O', 'Ö' }
            };

            return new string(input.Select(c => englishToTurkishMap.ContainsKey(c) ? englishToTurkishMap[c] : c).ToArray());
        }

        // Capitalizes the first letter of the string
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
