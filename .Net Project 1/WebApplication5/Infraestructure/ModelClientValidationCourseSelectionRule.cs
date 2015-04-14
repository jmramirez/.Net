using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Infraestructure
{
    public class ModelClientValidationCourseSelectionRule : ModelClientValidationRule 
    {
        public ModelClientValidationCourseSelectionRule(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ValidationType = "required_group";
        }
    }
}