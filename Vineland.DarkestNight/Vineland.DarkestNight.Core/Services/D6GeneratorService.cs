using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vineland.DarkestNight.Core.Services
{
    public class D6GeneratorService
    {
        Random _generator;

        public D6GeneratorService()
        {
            _generator = new Random();
        }

        public int RollDemBones()
        {
            return _generator.Next(1, 6);
        }
    }
}
