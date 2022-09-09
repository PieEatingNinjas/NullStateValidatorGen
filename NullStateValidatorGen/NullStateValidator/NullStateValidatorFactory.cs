//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace NullStateValidator
//{
//    public partial class NullStateValidatorFactory
//    {
//        IDictionary<string, object> NullStateValidators = new Dictionary<string, object>();

//        //static NullStateValidatorFactory()
//        //{
//        //    Init();
//        //}

//        partial void Init();
       
//        public INullStateValidator<T>? GetFor<T>()
//        {
//            Init();
//            if(NullStateValidators.ContainsKey(typeof(T).Name))
//            {
//                return (INullStateValidator<T>)NullStateValidators[typeof(T).Name];
//            }
//            return null;
//        }
//    }
//}
