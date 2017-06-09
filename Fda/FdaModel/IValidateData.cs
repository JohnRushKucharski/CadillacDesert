using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IValidateData: IValidatedData
    {
        bool Validate();
        IEnumerable<string> ReportValidationErrors();
    }
}
