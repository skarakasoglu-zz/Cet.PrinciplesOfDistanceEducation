using System;

namespace Cet.PrinciplesOfDistanceEducation.Models
{
    public class Bundle
    {
        public UserViewModel User { get; set; }
        public int[] VideoYears { get; set; }

        public Bundle()
        {
            User = null;
        }
    }

    public class Bundle<T> : Bundle
    {
        public T PageModel { get; set; }

        public Bundle(): base()
        {
            
        }

        public Bundle(T model): base()
        {
            PageModel = model;
        }
    }
}