using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.ViewModels
{
    public class VM_Error
    {
        public Dictionary<string, string> ErrorMessages { get; set; }

        public VM_Error GetErrorModel(FormCollection collection, ModelStateDictionary msd, string eMessage ="" )
        {
            ErrorMessages.Clear();
            if (eMessage != "") ErrorMessages["Exception"] = Convert.ToString(eMessage);

            var i = 0;
            foreach(var item in collection)
            {
                ErrorMessages[Convert.ToString(item)] = collection[i++];
            }
            if(collection.Count > 0)
            {
                ErrorMessages["Collection Count"] = Convert.ToString(collection.Count);
            }
            foreach(var item in msd.Values.SelectMany(v => v.Errors))
            {
                ErrorMessages["ModelStateError"] = item.ErrorMessage;
                if (item.Exception != null) ErrorMessages["ModelStateException"] = item.Exception.Message;
            }
            return this;
        }
        public VM_Error()
        {
            ErrorMessages = new Dictionary<string, string>();
        }
    }
}