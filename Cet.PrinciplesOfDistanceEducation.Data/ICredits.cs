using System;
using Cet.PrinciplesOfDistanceEducation.Data.Models;

namespace Cet.PrinciplesOfDistanceEducation.Data
{
    public interface ICredits
    {
        void Add(Credits credits);
        void Update(Credits credits);
        void Remove(Credits credits);

        string GetLastCreditsContent();

        void CreateCredits(string content, string userName);
    }
}