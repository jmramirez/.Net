using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Infraestructure
{

    public class ModelClientValidationRangeDateRule : ModelClientValidationRule
    {
        public ModelClientValidationRangeDateRule(string errorMessage, DateTime minValue, DateTime maxValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "rangedate";

            ValidationParameters["min"] = minValue.ToString("yyyy/MM/dd");
            ValidationParameters["max"] = maxValue.ToString("yyyy/MM/dd");
        }
    }

}