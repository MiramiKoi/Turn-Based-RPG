using System;
using System.Collections.Generic;

namespace Runtime.Descriptions.StatusEffects.Constraints
{
    public static class ConstraintDescriptionExtensions
    {
        
        public static ConstraintDescription ToConstraintDescription(this Dictionary<string, object> data, string type)
        {
            return type switch
            {
                _ =>  throw new NotImplementedException()
            };
        }
    }
}