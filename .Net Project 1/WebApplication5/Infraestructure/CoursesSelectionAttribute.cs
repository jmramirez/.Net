using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Infraestructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CoursesSelectionAttribute : ValidationAttribute, IClientValidatable
    {

        private const string DefaultErrorMessage = "Malo"; 

        public CoursesSelectionAttribute():base(DefaultErrorMessage)
        {

        }
        public override bool IsValid(object value)
        {
            bool valid = false;
            var collection = value as ICollection;
            if (collection != null)
            {
                if (collection.Count > 0)
                    valid = true;
            }
            return valid;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
        ModelMetadata metadata,
        ControllerContext context)
        {
            return new[]
                       {
                           new ModelClientValidationCourseSelectionRule(FormatErrorMessage(metadata.GetDisplayName()))
                       };
        }
    }
}