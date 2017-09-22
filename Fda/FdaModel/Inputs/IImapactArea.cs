using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs
{
    internal interface IImapactArea: IValidateData
    {
        string Name { get; }
        float ComputationPoint { get; }
    }
}
