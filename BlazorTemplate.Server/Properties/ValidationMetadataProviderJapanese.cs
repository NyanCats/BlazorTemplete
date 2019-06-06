using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Properties
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Resources;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

    namespace SampleProject.Resources
    {
        public class ValidationMetadataProviderJapanese : IValidationMetadataProvider
        {
            private ResourceManager resourceManager;
            private Type resourceType;

            public ValidationMetadataProviderJapanese(string baseName, Type type)
            {
                /*
                var types = new List<Type>();
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach( var type1 in asm.GetTypes())
                    {
                        if (type1.IsSubclassOf(typeof(ValidationAttribute)) && !type1.IsAbstract) types.Add(type1);
                    }
                }
                */
                resourceType = type;
                resourceManager = new ResourceManager(baseName,
                    type.GetTypeInfo().Assembly);
            }
            public void CreateValidationMetadata(
                ValidationMetadataProviderContext context)
            {
                if (context.Key.ModelType.GetTypeInfo().IsValueType &&
                    context.ValidationMetadata.ValidatorMetadata.Where(m => m.GetType() == typeof(RequiredAttribute)).Count() == 0)
                    context.ValidationMetadata.ValidatorMetadata.Add(new RequiredAttribute());

                foreach (var attribute in context.ValidationMetadata.ValidatorMetadata)
                {
                    ValidationAttribute tAttr = attribute as ValidationAttribute;
                    if (tAttr != null && tAttr.ErrorMessageResourceName == null)
                    {
                        //何故かEmailAddressAttributeはErrorMessageがデフォルトでnullにならない。
                        if (tAttr.ErrorMessage == null || (attribute as EmailAddressAttribute != null && !string.IsNullOrEmpty(tAttr.ErrorMessage)))
                        {
                            var name = tAttr.GetType().Name;
                            if (resourceManager.GetString(name) != null)
                            {
                                tAttr.ErrorMessageResourceType = resourceType;
                                tAttr.ErrorMessageResourceName = name;
                                tAttr.ErrorMessage = null;
                            }
                        }
                    }
                }
            }

        }
    }
}
